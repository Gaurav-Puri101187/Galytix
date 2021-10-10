This is the sample web api for galytix

1) Controllers contain the code to be exposed to FE clients and contains the basic HTTP infra
2) Domain contains all the domain logic and interaction with the repository layer
3) Repo project contains the code to hydrate itself with csv data at first call

Sample curl request

curl --location --request POST 'http://localhost:9091/api/countrygwp/avg/country/india' \
--header 'Content-Type: application/json' \
--header 'Cookie: rbi_cookies_accepted_nic=v1' \
--data-raw '[
    "Transport",
    "Liability"
]'

Sample 200 response 

{
    "Transport": 143.2875,
    "Liability": 138.5875
}

*** Run the solution ****
Open the solution ion VS2019/VSCode and select the Galytix profile and run this will run the solution on 9091 port which is configured
in program.cs

Or you can build the solution using ***dotnet build*** and then ***dotnet .\bin\debug\netcoreapp3.1\Galytix.dll***
