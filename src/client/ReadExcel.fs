module ReadExcel

open FSharp.Interop.Excel

type CompanyWorksheet = ExcelFile<"D:/Mauro.xlsx", HasHeaders=true>

let file = CompanyWorksheet()
let rows = file.Data |> Seq.map (fun row -> row.Company)
