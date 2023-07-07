﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace ASP.NETCoreIdentity.Areas.Identity.Data;

public class AccountUser : IdentityUser
{
    public string FirstName {  get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string MobileNo { get; internal set; }
    public string Address { get; internal set; }
    public string Province { get; internal set; }
    public string CityOrMunicipality { get; internal set; }
}

