global using System;
global using System.Collections.Generic;
global using System.Threading.Tasks;
global using System.Linq;
global using System.Globalization;

global using System.Data;

global using Azure.Core;
global using Azure.Identity;
global using Azure.Security.KeyVault.Secrets;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Caching.Memory;

global using Serilog;

global using Npgsql;

global using eSDSCom.Editor.Shared.DocumentElements;
global using eSDSCom.Editor.Shared.Models;

global using eSDSCom.Editor.Server.Brokers;
global using eSDSCom.Editor.Server.Exceptions;
