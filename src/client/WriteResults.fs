module WriteResults

open System.IO
open CompanyInfo

let writeToFile (info: seq<CompanyInfo>): Unit =
    let wr = new StreamWriter("CompanyData2.csv")
    wr.WriteLine("Company,SIREN,SIRET,Adress,CITY,ZIP")
    info
    |> Seq.map (string)
    |> String.concat ("\n")
    |> wr.WriteLine
    wr.Close()
