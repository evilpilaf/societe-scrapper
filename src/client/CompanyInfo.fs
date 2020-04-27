module CompanyInfo

type CompanyInfo =
    { Name: string
      SIREN: string
      ``SIRET (siege)``: string
      Adresse: string
      Ville: string
      ``Code Postal``: string }

    override this.ToString() =
        sprintf "%s,%s,%s,%s,%s,%s" this.Name this.SIREN this.``SIRET (siege)`` this.Adresse this.Ville
            this.``Code Postal``

let valueOrDefault (value: string) (infoMap: Map<string, string>): string =
    match infoMap.TryFind value with
    | Some x -> x
    | None -> "No value found"

let parse (infoMap: Map<string, string>) =
    { Name = valueOrDefault "Company" infoMap
      SIREN = valueOrDefault "SIREN" infoMap
      ``SIRET (siege)`` = valueOrDefault "SIRET (siege)" infoMap
      Adresse = valueOrDefault "Adresse" infoMap
      Ville = valueOrDefault "Ville" infoMap
      ``Code Postal`` = valueOrDefault "Code postal" infoMap }
