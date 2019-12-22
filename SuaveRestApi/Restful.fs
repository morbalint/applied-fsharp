namespace SuaveRestApi.Rest

[<AutoOpen>]
module RestFul =

    open Newtonsoft.Json

    type RestResource<'a> =
        { GetAll: unit -> 'a seq }

    // string -> RestResource<'a> -> WebPart
    let rest resourceName resource =
        // TODO
        0

    let JSON v =
        let settings = new JsonSerializerSettings()
        settings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(v, settings)
        |> OK
        >=> Writers.setMimeType "application/json; charset=utf-8"
"