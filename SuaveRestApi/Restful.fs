namespace SuaveRestApi.Rest

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave
open Suave.Operators
open Suave.Http
open Suave.Successful

[<AutoOpen>]
module RestFul =

    open Suave.RequestErrors
    open Suave.Filters

    type RestResource<'a> =
        { GetAll: unit -> 'a seq
          Create: 'a -> 'a
          Update: 'a -> 'a option
          Delete: int -> unit }

    let JSON v =
        let settings = JsonSerializerSettings()
        settings.ContractResolver <- CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(v, settings)
        |> OK
        >=> Writers.setMimeType "application/json; charset=utf-8"

    let fromJson<'a> json = JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a

    let getResourceFromReq<'a> (req: HttpRequest) =
        req.rawForm
        |> System.Text.Encoding.UTF8.GetString
        |> fromJson<'a>

    let handleResource requestError op =
        match op with
        | Some r -> r |> JSON
        | _ -> requestError

    let rest resourceName resource =
        let resourcePath = "/" + resourceName

        let resourceIdPath =
            let path = resourcePath + "/%d"
            new PrintfFormat<int -> string, unit, string, string, int>(path)
        let deleteResourceById id =
            resource.Delete id
            NO_CONTENT

        let getAll = fun _ -> resource.GetAll() |> JSON
        path resourcePath >=> choose
                                  [ GET >=> (warbler getAll)
                                    POST >=> request
                                                 (getResourceFromReq
                                                  >> resource.Create
                                                  >> JSON)
                                    PUT >=> request
                                                (getResourceFromReq
                                                 >> resource.Update
                                                 >> handleResource (BAD_REQUEST ""))
                                    DELETE >=> pathScan resourceIdPath deleteResourceById ]
