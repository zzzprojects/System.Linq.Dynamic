using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Globalization;
using System.Linq.Expressions;

namespace System.Linq.Dynamic.Test
{
    [TestClass]
    public class DynamicExpressionCultureTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-PT");
        }

        [TestMethod]
        public void Parse_DoubleLiteral_ReturnsDoubleExpression()
        {
            var expression = (ConstantExpression)DynamicExpression.Parse(typeof(double), "1.0");
            Assert.AreEqual(typeof(double), expression.Type);
            Assert.AreEqual(1.0, expression.Value);
        }

        [TestMethod]
        public void Parse_FloatLiteral_ReturnsFloatExpression()
        {
            var expression = (ConstantExpression)DynamicExpression.Parse(typeof(float), "1.0f");
            Assert.AreEqual(typeof(float), expression.Type);
            Assert.AreEqual(1.0f, expression.Value);
        }
    }
}
﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Dynamic.Test
{
    [TestClass]
    public class DynamicExpressionTests
    {
        [TestMethod]
        public void Parse_ParameterExpressionMethodCall_ReturnsIntExpression()
        {
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(int), "x") },
                typeof(int),
                "x + 1");
            Assert.AreEqual(typeof(int), expression.Type);
        }

        [TestMethod]
        public void Parse_TupleToStringMethodCall_ReturnsStringLambdaExpression()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(Tuple<int>),
                typeof(string),
                "it.ToString()");
            Assert.AreEqual(typeof(string), expression.ReturnType);
        }

        [TestMethod]
        public void Parse_StringLiteral_ReturnsBooleanLambdaExpression()
        {
            var expression = DynamicExpression.Parse(new[] { Expression.Parameter(typeof(string), "Property1") }, typeof(Boolean), "Property1 == \"test\"");
            Assert.AreEqual(typeof(Boolean), expression.Type);
        }

        [TestMethod]
        public void Parse_StringLiteralEmpty_ReturnsBooleanLambdaExpression()
        {
            var expression = DynamicExpression.Parse(new[] { Expression.Parameter(typeof(string), "Property1") }, typeof(Boolean), "Property1 == \"\"");
            Assert.AreEqual(typeof(Boolean), expression.Type);
        }

        [TestMethod]
        public void Parse_StringLiteralEmbeddedQuote_ReturnsBooleanLambdaExpression()
        {
            string expectedRightValue = "\"test \\\"string\"";
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(string), "Property1") }, 
                typeof(Boolean), 
                string.Format("Property1 == {0}", expectedRightValue));

            string rightValue = ((BinaryExpression) expression).Right.ToString();
            Assert.AreEqual(typeof(Boolean), expression.Type);
            Assert.AreEqual(expectedRightValue, rightValue);
        }

        [TestMethod]
        public void Parse_StringLiteralStartEmbeddedQuote_ReturnsBooleanLambdaExpression()
        {
            string expectedRightValue = "\"\\\"test\"";
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(string), "Property1") },
                typeof(Boolean),
                string.Format("Property1 == {0}", expectedRightValue));

            string rightValue = ((BinaryExpression)expression).Right.ToString();
            Assert.AreEqual(typeof(Boolean), expression.Type);
            Assert.AreEqual(expectedRightValue, rightValue);
        }

        [ExpectedException(typeof(ParseException))]
        [TestMethod]
        public void Parse_StringLiteral_MissingClosingQuote()
        {
            string expectedRightValue = "\"test\\\"";
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(string), "Property1") },
                typeof(Boolean),
                string.Format("Property1 == {0}", expectedRightValue));
        }

        [TestMethod]
        public void Parse_StringLiteralEscapedBackslash_ReturnsBooleanLambdaExpression()
        {
            string expectedRightValue = "\"test\\string\"";
            var expression = DynamicExpression.Parse(
                new[] { Expression.Parameter(typeof(string), "Property1") },
                typeof(Boolean),
                string.Format("Property1 == {0}", expectedRightValue));

            string rightValue = ((BinaryExpression)expression).Right.ToString();
            Assert.AreEqual(typeof(Boolean), expression.Type);
            Assert.AreEqual(expectedRightValue, rightValue);
        }

        [TestMethod]
        public void ParseLambda_DelegateTypeMethodCall_ReturnsEventHandlerLambdaExpression()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(EventHandler),
                new[] { Expression.Parameter(typeof(object), "sender"),
                        Expression.Parameter(typeof(EventArgs), "e") },
                null,
                "sender.ToString()");

            Assert.AreEqual(typeof(void), expression.ReturnType);
            Assert.AreEqual(typeof(EventHandler), expression.Type);
        }

        [TestMethod]
        public void ParseLambda_VoidMethodCall_ReturnsActionDelegate()
        {
            var expression = DynamicExpression.ParseLambda(
                typeof(System.IO.FileStream),
                null,
                "it.Close()");
            Assert.AreEqual(typeof(void), expression.ReturnType);
            Assert.AreEqual(typeof(Action<System.IO.FileStream>), expression.Type);
        }

        [TestMethod]
        public void CreateClass_TheadSafe()
        {
            const int numOfTasks = 15;

            var properties = new[] { new DynamicProperty("prop1", typeof(string)) };

            var tasks = new List<Task>(numOfTasks);

            for (var i = 0; i < numOfTasks; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => DynamicExpression.CreateClass(properties)));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
﻿<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProductVersion>
    </ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{66D3514B-0E04-4C85-879D-67E15FBD0E37}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>System.Linq.Dynamic.Test</RootNamespace>
    <AssemblyName>System.Linq.Dynamic.Test</AssemblyName>
    <TargetFrameworkVersion>v4.0</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <ProjectTypeGuids>{3AC096D0-A1C2-E12C-1390-A8335801FDAB};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="Microsoft.VisualStudio.QualityTools.UnitTestFramework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL">
      <Private>False</Private>
    </Reference>
    <Reference Include="System" />
    <Reference Include="System.Core">
      <RequiredTargetFramework>3.5</RequiredTargetFramework>
    </Reference>
  </ItemGroup>
  <ItemGroup>
    <CodeAnalysisDependentAssemblyPaths Condition=" '$(VS100COMNTOOLS)' != '' " Include="$(VS100COMNTOOLS)..\IDE\PrivateAssemblies">
      <Visible>False</Visible>
    </CodeAnalysisDependentAssemblyPaths>
  </ItemGroup>
  <ItemGroup>
    <Compile Include="DynamicExpressionCultureTests.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
    <Compile Include="DynamicExpressionTests.cs" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\System.Linq.Dynamic\System.Linq.Dynamic.csproj">
      <Project>{b6edf157-6153-4684-a577-de33896dbaa8}</Project>
      <Name>System.Linq.Dynamic</Name>
    </ProjectReference>
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\System.Linq.Dynamic\System.Linq.Dynamic.csproj">
      <Project>{b6edf157-6153-4684-a577-de33896dbaa8}</Project>
      <Name>System.Linq.Dynamic</Name>
    </ProjectReference>
  </ItemGroup>
  <Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name="BeforeBuild">
  </Target>
  <Target Name="AfterBuild">
  </Target>
  -->
</Project>