module CompanySearch

open FSharp.Data

let baseUrl = "https://www.societe.com/cgi-bin/search"

let getCompanyResults companyName =
    let response = Http.RequestString(baseUrl, query = [ "champs", companyName ])
    HtmlDocument.Parse(response)

let isResultLink (node: HtmlNode) =
    let resultNodes = node.Descendants [ "div" ] |> Seq.where (fun x -> x.HasClass("resultat"))
    not (Seq.isEmpty resultNodes)


let searchForCompany (doc: HtmlDocument) =
    let link = doc.Descendants [ "a" ] |> Seq.filter isResultLink
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

// let downloadCompanyInfo link =
//     let response = Http.RequestString()
