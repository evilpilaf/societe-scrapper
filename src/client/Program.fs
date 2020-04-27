open CompanySearch
open CompanyInfo
open WriteResults
open ReadExcel

let addCompanyInfo (currentInfo: Map<string, string>) (propertyName, propertyValue) =
    currentInfo.Add(propertyName, propertyValue)

let getCompanyInfo companyName =
    if companyName = "" then
        None
    else
        let searchResult = getCompanyResults companyName |> searchForCompany
        let b = match searchResult with
                | Some x ->
                    downloadCompanyInfo x
                    |> Seq.fold addCompanyInfo Map.empty
                | None -> Map.empty
        let info = b.Add("Company", companyName)
        Some(parse info)

let foldCompanyInfo currentInfo companyInfo=
    match companyInfo with
    | Some x -> Seq.append currentInfo [x]
    | None -> currentInfo

[<EntryPoint>]
let main args =
    let companyInfo:seq<CompanyInfo> = rows |> Seq.fold (fun a row ->
            printfn "Now processing %s" row
            let companyInfo = getCompanyInfo row
            companyInfo |> foldCompanyInfo a) Seq.empty
    writeToFile companyInfo
    0