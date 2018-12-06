# autosite

Live demo:
http://autosite.nevillenazerane.com/

Running locally:

1. Setup the following
    - A Sql Server database (update ef migrations for any other db)
    - QnA maker 
    - LUIS
    - Computer vision 
    - Custom vision
  
 2. You can train all required cognitive services form the contents in this dropbox link: https://www.dropbox.com/sh/yx6k5tzf8ncb62y/AACuyqbDNIdW8GW14fUeIU1ka?dl=0 
  
 2. Use the following format for the secret manager:
 
 ```
 {
  "connectionStrings": {
    "sqlServer": ""
  },
  "QnA": {
    "base": "",
    "typeMapper": {
      "path": "knowledgebases/..../generateAnswer",
      "Authorization": "EndpointKey ...."
    }
  },
  "cognitive": {
    "luisBase": "https://westus.api.cognitive.microsoft.com/luis/v2.0/",
    "entityUnderstanding": {
      "path": "apps/....",
      "subscription-key": ""
    }
  },
  "vision": {
    "endPoint": "https://eastus.api.cognitive.microsoft.com/",
    "key": ""
  },
  "customVision": {
    "base": "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/----/",
    "imagePath": "image?iterationId=.....",
    "key": ""
  }
}
 ```
 3. To setup database, open command line on the AutoSite.Migrations folder and run `dotnet ef database update` in CLI. 
 
 4. To install the tool locally, CD into the make-autosite folder and use the following CLI:
 ```
 dotnet pack
 dotnet tool install make-autosite -g --add-source bin\release
 ```
 Remember to have the website running while testing the tool.
 After initial installation you can update using the install.cmd batch file. 
