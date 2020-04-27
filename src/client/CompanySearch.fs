module CompanySearch

open System
open FSharp.Data

let baseUrl = "https://www.societe.com/cgi-bin/search"
let parseHtml x = HtmlDocument.Parse(x)
let requestHtml url query = Http.RequestString(url, query = query)


let sanitize (value: string) = value.Replace(" ", "+").Replace("\.", "").Trim()

let getCompanyResults companyName =
    try
        let response = requestHtml baseUrl [ "champs", sanitize companyName ]
        response
        |> parseHtml
        |> Some

    with _ -> None

let isResultLink (node: HtmlNode) =
    try
        let resultNodes = node.Descendants [ "div" ] |> Seq.where (fun x -> x.HasClass("resultat"))
        not (Seq.isEmpty resultNodes)
    with _ -> false


let searchForCompany (doc: option<HtmlDocument>) =
    match doc with
    | Some x ->
        let link = x.Descendants [ "a" ] |> Seq.filter isResultLink
        match Seq.length link with
        | 1 ->
            let a = Seq.tryHead link
            match a with
            | Some b ->
                match b.TryGetAttribute("href") with
                | None -> None
                | Some att -> Some(att.Value())
            | None -> None
        | _ -> None
    | None -> None

type CompanyData =
    HtmlProvider<"https://www.societe.com/societe/coca-cola-european-partners-deutschland-gmbh-880669734.html">

let getBasicInfo (data: CompanyData) (fieldName: List<string>) =
    data.Tables.``Renseignements juridiques``.Rows
    |> Seq.filter (fun row -> List.contains row.Column1 fieldName)
    |> Seq.map (fun r -> r.Column1, r.Column2)

let getExtendedInfo (data: CompanyData) (fieldName: List<string>) =
    data.Tables.``Renseignements juridiques 2``.Rows
    |> Seq.filter (fun row -> List.contains row.Column1 fieldName)
    |> Seq.map (fun r -> r.Column1, r.Column2)

let downloadCompanyInfo link =
    let requestUrl = sprintf "https://www.societe.com/%s" link
    let data = CompanyData.Load(requestUrl)
    let basicInfo = getBasicInfo data [ "SIREN"; "SIRET (siege)" ]
    let extendedInfo = getExtendedInfo data [ "Adresse"; "Code postal"; "Ville" ]
    Seq.append basicInfo extendedInfo
