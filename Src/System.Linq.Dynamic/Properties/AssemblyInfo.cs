using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: InternalsVisibleTo("System.Linq.Dynamic.Tests, PublicKey=002400000480000094000000060200000024000052534131000400000100010037361a01268ef18864bd28306b22d50e84438d363923e1a6414759238d004114acc53c4e69886d948dd5125963b10a45a6834c4dc6baeb1534a69a7582531816535aa43824ea095ac80a687fe27c0c49dd72e4b5bec580fa0e12aa35df208fb8dc4a4cdfb8c8f1598ed26cf38bfb5ae8eea5ed79955470c792c7a40250772b9b")]
[assembly: InternalsVisibleTo("System.Linq.Dynamic.For35.Tests, PublicKey=002400000480000094000000060200000024000052534131000400000100010037361a01268ef18864bd28306b22d50e84438d363923e1a6414759238d004114acc53c4e69886d948dd5125963b10a45a6834c4dc6baeb1534a69a7582531816535aa43824ea095ac80a687fe27c0c49dd72e4b5bec580fa0e12aa35df208fb8dc4a4cdfb8c8f1598ed26cf38bfb5ae8eea5ed79955470c792c7a40250772b9b")]
#endif


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("System.Linq.Dynamic")]
[assembly: AssemblyCompany("Nathan Arnott")]
[assembly: AssemblyProduct("System.Linq.Dynamic")]

#if SILVERLIGHT
[assembly: AssemblyConfiguration("Silverlight")]
[assembly: AssemblyDescription("Adds support for Dynamic Linq string expressions, rather than lambda expressions. For Silverlight.")]
#elif NET35
[assembly: AssemblyConfiguration("Net35")]
[assembly: AssemblyDescription("Adds support for Dynamic Linq string expressions, rather than lambda expressions. For .Net 3.5 Framework.")]
#else
[assembly: AssemblyConfiguration("Net40")]
[assembly: AssemblyDescription("Adds support for Dynamic Linq string expressions, rather than lambda expressions. For .Net 4/4.5+ Framework.")]
#endif


[assembly: CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.1.13.*")]
