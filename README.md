# esdscom-editor

The esdscom-editor project is an open source development effort aimed at giving companies an editor toolset for importing, updating and exporting DatasheetFeeds (called Document Sets in the app) among their business partners.

This project is in its infancy (pre-pre-Alpha!) and under active development. 

The project is based on the **.Net Blazor Web Assembly (WASM) project type.**  The Blazor WASM technology is quite new but seems well-suited to document editing use cases.

It includes **.Net6.0 and C#10** features so if you want to fork,clone or otherwise kick the tires of it you'll need to make sure you have thoese resources available. The solution should work within the Visual Studio 2022 Preview (latest) and Visual Studio Code IDEs.

The editor uses the latest version of the eSDSComXML standard (5.4), the latest set of phrases and a subset of substances downloaded from ECHA. 

The solution is made up of the following projects:

**Client** - this contains the pages that make up the editor web site. Since this is a WASM style project the project also contains a fair amount of C# - usually contained with @code blocks on each page, but there may be classes added from time to time.  There is no session state management, ther AppState component (in the /Shared folder) uses Blazor functionality to maintain user state. The AppState component must be added as a CascadingParameter for each page wishing to work with user state. All Client traffic to the Database is managed by the DataManager class.

**Server** - this contains the API Controllers that intereact with the Client pages. The pattern in place keeps the controller logic quite simple - the main role of a controller is to act as a pass through for HTTP traffic between one or more Brokers and the Client project. 

**Shared** - this contains the models that are used by both the Client and Server projects. There are also some shared classes that expose functionality to the Client and Server projects.

The FixXML and xsdNavigator projects are not part of the application but may be useful at some point.

# how the editor works

The esdscom standard is complex and the supporting xml schemas reflect this complexity and the content contunues to evolve (a good thing!).

The philosophy behind the editor is to present a dynamic interface for editing the current esdscom version with the current phrase catalog.

In order to do this there is a one-time process that runs against all of the xml schema files associated with the escomxml standard. This process extracts the simple and complex
types defined in the standard and organizes them into a SchemaElements.xml file. The SchemaElements.xml file is distributed as part of the Client and lives in the /Data folder. The schema files also define a number of enumerations that describe the allowed choices for a particular schema element. This data is also extracted from the escomxml standard files and saved to an EnumValues.xml file, also in the Client /Data folder.

When the application is launched (the Server project is the startup project) The SchemaElement and EnumValues files form the basis of datasheet editing. The Client project contains an element renderer component (/Pages/Components/ComplexTypeComponent.razor)  that interogates each of the Section's structure and dynamically renders components that support the type of elements that are defined in the Section. So an element defined as a 'string:128' type in the schema is presented to the user as an input component with the appropriate max length, validation rules and help text (if any). Likewise an element defined as 'xs:date' in the schema is presented as a standard date picker control. Since the schema is made up of many,many complex types with various levels of nesting and regional differences there is A LOT of work to do in order to get this right, but the foundation is there.

## Contributors welcome!
