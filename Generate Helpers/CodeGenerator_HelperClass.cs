﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace XSDCustomToolVSIX.Generate_Helpers
{
    internal class CodeGenerator_HelperClass : CodeGenerator_Base
    {

        internal CodeGenerator_HelperClass(ParsedFile parsedFile) : base(parsedFile) { }

        #region < Properties >

        /// <summary> Location of the _HelperClass file on disk. </summary>
        public override FileInfo FileOnDisk => new FileInfo(ParsedFile.xSD_Instance.InputFile.FullName.Replace(".xsd", $"_HelperClass{ParsedFile.OutputFileExtension}"));

        /// <summary> Name of the output class when generating the helper file. </summary>
        public virtual string OutputClassName => this.ParsedFile.xSD_Instance.InputFile.Name.Replace(".xsd", "_HelperClass");

        #endregion </ Properties >

        #region < Methods >

        public override void Generate()
        {
            //Clone the structor that XSD.exe built, stripping away the attributes as needed
            int i1 = ParsedFile.ParsedCode.Namespaces.Count;
            CodeNamespace PNS = ParsedFile.ParsedCode.Namespaces[0];
            CodeCompileUnit OutputFile = new CodeCompileUnit();
            CodeNamespace GlobalNameSpace = new CodeNamespace();
            CodeNamespace @namespace = new CodeNamespace(PNS.Name);
            OutputFile.Namespaces.Add(GlobalNameSpace);
            OutputFile.Namespaces.Add(@namespace);
            //Add the required Imports and Comments
            AddImports(GlobalNameSpace);
            @namespace.Comments.AddRange(GetComment_AutoGen());

            //Generate Helper Class
            CodeTypeDeclaration @class = new CodeTypeDeclaration()
            {
                IsPartial = true,
                IsClass = true,
                IsEnum = false,
                IsInterface = false,
                IsStruct = false,
                Name = OutputClassName,
                Attributes = MemberAttributes.Public,
                TypeAttributes = System.Reflection.TypeAttributes.Class & System.Reflection.TypeAttributes.Public
            };

            //Helper Class Summary
            @class.Comments.Add($"<summary>");
            @class.Comments.Add($"Helper class to ease working with {this.XSDInstance.InputFile.Name} autogenerated {(IsGeneratingClass ? "class" : "dataset")}");
            @class.Comments.Add($"</summary>");

            GenerateConstructors(@class);   //Constructors
            GenerateProperties(@class);     //Properties and Fields
            GenerateSaverAndLoader(@class); //XML Serialization

            base.Save(OutputFile);
        }

        /// <summary>
        /// Adds the following Imports to the NameSpace <br/>
        /// System<br/>
        /// System.IO<br/>
        /// System.Xml.Serialization
        /// </summary>
        /// <param name="NameSpace"></param>
        protected virtual void AddImports(CodeNamespace @NameSpace)
        {
            @NameSpace.Imports.Add(new CodeNamespaceImport("System"));
            @NameSpace.Imports.Add(new CodeNamespaceImport("System.IO"));
            @NameSpace.Imports.Add(new CodeNamespaceImport("System.Xml.Serialization"));
        }

        #region < Constructors >

        /// <summary>Generate the Constructors and add them to the helper class</summary>
        /// <remarks>Adds the results of the following methods to the class members: <br/>
        /// GenerateConstructor_Parameterless
        /// GenerateConstructor_FilePathArg
        /// GenerateConstructor_DeserializedXML
        /// </remarks>
        protected virtual void GenerateConstructors(CodeTypeDeclaration @class)
        {
            CodeConstructor tmp = GenerateConstructor_Parameterless();
            tmp.StartDirectives.Add(StartRegion_Constructor());
            @class.Members.Add(tmp);
            
            @class.Members.Add(GenerateConstructor_FilePathArg());

            tmp = GenerateConstructor_DeserializedXML();
            tmp.StartDirectives.Add(EndRegion_Constructor());
            @class.Members.Add(tmp);
        }

        protected virtual CodeConstructor GenerateConstructor_Parameterless() 
        {
            CodeConstructor cstr = new CodeConstructor();
            cstr.Attributes = MemberAttributes.Public;
            cstr.Comments.Add($"<summary> Construct a new instance of the {OutputClassName} object. </summary>");
            cstr.Statements.Add(new CodeCommentStatement("TO DO: assign values for all the properties"));
            return cstr;
        }

        protected virtual CodeConstructor GenerateConstructor_FilePathArg() 
        {
            CodeConstructor cstr = new CodeConstructor();
            cstr.Attributes = MemberAttributes.Public;
            cstr.Comments.Add($"<summary> Construct a new instance of the {OutputClassName} object by Deserializing an XML file. </summary>");
            cstr.Comments.Add($"<param name=\"FilePath\"> This XML file to read into the class object </param>");
            cstr.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "FilePath"));
            cstr.Statements.Add(new CodeAssignStatement(
                left: new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), TopLevelClass?.HelperClass_PropertyName ?? "FAULT"),
                right: new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "Load", new CodeVariableReferenceExpression("FilePath"))
                ));
            return cstr;
        }
        
        protected virtual CodeConstructor GenerateConstructor_DeserializedXML() 
        {
            CodeConstructor cstr = new CodeConstructor();
            cstr.Attributes = MemberAttributes.Public;
            cstr.Comments.Add($"<summary> Construct a new instance of the {OutputClassName} object from an existing <typeparamref name=\"{TopLevelClass.ClassName}\"/> object. </summary>");
            cstr.Comments.Add($"<param name=\"{TopLevelClass.HelperClass_PropertyName.ToLower()}\"> A pre-existing <typeparamref name=\"{TopLevelClass.ClassName}\"/> object.</param>");
            string varRef = TopLevelClass.HelperClass_PropertyName.ToLower_FirstCharOnly();
            cstr.Parameters.Add(new CodeParameterDeclarationExpression(TopLevelClass.ClassName, varRef));
            cstr.Statements.Add(new CodeAssignStatement(
                left: new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), TopLevelClass?.HelperClass_PropertyName ?? "FAULT"),
                right: new CodeVariableReferenceExpression(varRef)
                ));                
            return cstr;
        }

        #endregion </ Constructors >

        #region < Properties >

        /// <summary></summary>
        protected virtual void GenerateProperties(CodeTypeDeclaration @class) 
        {
            CodeMemberProperty tmp = TopLevelClass.GetHelperClassProperty();
            tmp.StartDirectives.Add(StartRegion("Properties"));
            tmp.EndDirectives.Add(EndRegion("Properties"));
            @class.Members.Add(tmp);
        }

        #endregion </ Properties >

        #region < Saving and Loading >

        protected virtual void GenerateSaverAndLoader(CodeTypeDeclaration @class) 
        {
            CodeMemberMethod load = GetClassLoaderMethod();
            CodeMemberMethod save = GetClassSaverMethod();

            load.StartDirectives.Add(StartRegion("Saving & Loading XML Files"));
            save.EndDirectives.Add(EndRegion("Saving & Loading XML Files"));
            @class.Members.Add(load);
            @class.Members.Add(save);
        }

        /// <summary> Generate a Load(string) method to deserialize an XML file into this helper class. </summary>
        /// <returns></returns>
        protected virtual CodeMemberMethod GetClassLoaderMethod()
        {
            // Using Statement isn't  available in all languages : use Try{}Catch{}Finally{} instead
            List<CodeStatement> TryStatements = new List<CodeStatement>();
            List<CodeStatement> FinallyStatements = new List<CodeStatement>();
            CodeCatchClause CatchClause = new CodeCatchClause("e", new CodeTypeReference(typeof(Exception)));
            CatchClause.Statements.Add(new CodeThrowExceptionStatement(new CodeVariableReferenceExpression("e")));
            
            CodeMemberMethod tmp = new CodeMemberMethod();
            tmp.Name = "LoadXmlFile";
            tmp.Comments.AddRange(GetComment_LoadMethod());
            tmp.ReturnType = TopLevelClass.GetCodeTypeReference();
            tmp.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "FilePath"));

            #region < Create Stream and Return Objects>
            CodeVariableReferenceExpression RetObjRef = new CodeVariableReferenceExpression("retObj");
            CodeVariableReferenceExpression StreamObjRef = new CodeVariableReferenceExpression("streamObj");
            tmp.Statements.Add(new CodeVariableDeclarationStatement(
                type: TopLevelClass.GetCodeTypeReference(),
                name: RetObjRef.VariableName,
                initExpression: new CodePrimitiveExpression(null)
                ));
            tmp.Statements.Add(new CodeVariableDeclarationStatement(
                type: new CodeTypeReference(typeof(Stream)),
                name: StreamObjRef.VariableName,
                initExpression: new CodePrimitiveExpression(null)
                ));
            #endregion

            #region < Open File Stream >
            TryStatements.Add(new CodeAssignStatement(StreamObjRef,
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(File)), "Open"),
                    new CodeExpression[]
                    {
                            new CodeVariableReferenceExpression("FilePath"),
                            new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(FileMode)), "Open")
                    })));
            #endregion

            #region < Create XMLSerializer and Deserialize > 

            //XmlSerializer serializer = new XmlSerializer(typeof({TopLevelClass.ClassName}
            CodeTypeReference xmlSer = new CodeTypeReference(typeof(System.Xml.Serialization.XmlSerializer));
            CodeVariableReferenceExpression SerObjRef = new CodeVariableReferenceExpression("serializer");
            TryStatements.Add(new CodeVariableDeclarationStatement(
                type: xmlSer,
                name: SerObjRef.VariableName,
                initExpression: new CodeObjectCreateExpression(xmlSer,
                new CodeExpression[]
                {
                        new CodeTypeOfExpression(TopLevelClass.GetCodeTypeReference())
                })
                ));
            

            //Deserialize into the return object
            TryStatements.Add(new CodeAssignStatement(
                left: RetObjRef,
                right: new CodeCastExpression(
                    targetType: TopLevelClass.GetCodeTypeReference(),
                    expression: new CodeMethodInvokeExpression(SerObjRef, "Deserialize", new CodeExpression[] { StreamObjRef })
                    )));

            #endregion
            
            #region < Finally Statements >
            //if (Stream != null) Stream.Dispose()
            CodeExpression IsNotNull = new CodeBinaryOperatorExpression(
                left: StreamObjRef,
                op: CodeBinaryOperatorType.IdentityInequality,
                right: new CodePrimitiveExpression(null)
                );
            FinallyStatements.Add(new CodeConditionStatement(
                IsNotNull,
                new CodeStatement[]{
                     new CodeExpressionStatement(new CodeMethodInvokeExpression(StreamObjRef, "Dispose", new CodeExpression[]{ }))
                }));
            #endregion

            //Wrap the statements into a TryCatch Statement
            tmp.Statements.Add(new CodeTryCatchFinallyStatement(TryStatements.ToArray(), new CodeCatchClause[] { CatchClause }, FinallyStatements.ToArray()));
            tmp.Statements.Add(new CodeMethodReturnStatement(RetObjRef));
            return tmp;
        }

        /// <summary> Generate a Save(string) method to serialize an XML file from this class. </summary>
        /// <returns></returns>
        protected virtual CodeMemberMethod GetClassSaverMethod()
        {
            // Using Statement isn't  available in all languages : use Try{}Catch{}Finally{} instead
            List<CodeStatement> TryStatements = new List<CodeStatement>();
            List<CodeStatement> FinallyStatements = new List<CodeStatement>();
            CodeCatchClause CatchClause = new CodeCatchClause("e", new CodeTypeReference(typeof(Exception)));
            CatchClause.Statements.Add(new CodeThrowExceptionStatement(new CodeVariableReferenceExpression("e")));
            
            CodeMemberMethod tmp = new CodeMemberMethod();
            tmp.Name = "SaveXMLFile"; 
            tmp.Comments.AddRange(GetComment_SaveMethod());
            tmp.ReturnType = new CodeTypeReference(typeof(void));
            tmp.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "FilePath"));
            
            #region < Stream Object >
            // Stream StreamObj  = null;
            CodeVariableReferenceExpression StreamObjRef = new CodeVariableReferenceExpression("streamObj");
            tmp.Statements.Add(new CodeVariableDeclarationStatement(
                type: new CodeTypeReference(typeof(Stream)),
                name: StreamObjRef.VariableName,
                initExpression: new CodePrimitiveExpression(null)
                ));
            #endregion

            #region < Create Dir >

            //Directory.CreateDirectory(new FileInfo(FilePath).DirectoryName)

            CodeExpression FileInfoObjectExpression = new CodeObjectCreateExpression(typeof(FileInfo), new CodeVariableReferenceExpression("FilePath"));
            TryStatements.Add(new CodeExpressionStatement(new CodeMethodInvokeExpression(
                targetObject: new CodeTypeReferenceExpression(typeof(Directory)),
                methodName: "CreateDirectory",
                parameters: new CodeExpression[]
                {
                    new CodePropertyReferenceExpression(FileInfoObjectExpression, "DirectoryName")
                })));

            #endregion

            #region < Create XMLSerializer > 
            //XmlSerializer serializer = new XmlSerializer(typeof({TopLevelClass.ClassName}

            CodeTypeReference xmlSer = new CodeTypeReference(typeof(System.Xml.Serialization.XmlSerializer));
            CodeVariableReferenceExpression SerObjRef = new CodeVariableReferenceExpression("serializer");
            TryStatements.Add(new CodeVariableDeclarationStatement(
                type: xmlSer,
                name: SerObjRef.VariableName,
                initExpression: new CodeObjectCreateExpression(xmlSer,
                new CodeExpression[]
                {
                        new CodeTypeOfExpression(TopLevelClass.GetCodeTypeReference())
                }
            )));
            #endregion

            #region < Write To Stream >
            // serializer.Serialize(stream, this.{TopLevelClass.HelperClass_PropertyName}); 
            TryStatements.Add(new CodeExpressionStatement(
                new CodeMethodInvokeExpression(
                targetObject: SerObjRef,
                methodName: "Serialize",
                parameters: new CodeExpression[]
                {
                    StreamObjRef,
                    new CodePropertyReferenceExpression(new  CodeThisReferenceExpression(), TopLevelClass.HelperClass_PropertyName)
                })));

            // stream.Flush();
            TryStatements.Add(new CodeExpressionStatement(
                new CodeMethodInvokeExpression(
                targetObject: StreamObjRef,
                methodName: "Flush",
                parameters: new CodeExpression[] {}
                )));

            #endregion

            #region < Finally Statements >
            //if (Stream != null) Stream.Dispose()
            CodeExpression IsNotNull = new CodeBinaryOperatorExpression(
                left: StreamObjRef,
                op: CodeBinaryOperatorType.IdentityInequality,
                right: new CodePrimitiveExpression(null)
                );
            FinallyStatements.Add(new CodeConditionStatement(
                IsNotNull,
                new CodeStatement[]{
                     new CodeExpressionStatement(new CodeMethodInvokeExpression(StreamObjRef, "Dispose", new CodeExpression[]{ }))
                }));

            #endregion
            
            //Wrap the statements into a TryCatch Statement
            tmp.Statements.Add(new CodeTryCatchFinallyStatement(TryStatements.ToArray(), new CodeCatchClause[] { CatchClause }, FinallyStatements.ToArray()));
            return tmp;

        }

        #endregion </ Saving and Loading >

        #endregion </ Methods >

        #region < Comment Methods >

        /// <summary>Generates the comment header of the file that says the file was automatically generated and when it will be overwritten.</summary>
        /// <returns>There are 3 Environment.NewLine keys inserted at the end of this comment.</returns>
        protected override CodeCommentStatementCollection GetComment_AutoGen()
        {
            CodeCommentStatementCollection CommentBlock = new CodeCommentStatementCollection();
            CommentBlock.Add("------------------------------------------------------------------------------");
            CommentBlock.Add($"<auto-generated>");
            CommentBlock.Add($"   This code was generated by XSDCustomTool VisualStudio Extension.");
            CommentBlock.Add($"   This file is only generated if it is missing, so it is safe to modify this file as needed.");
            CommentBlock.Add($"   If the file is renamed or deleted, then it will be regenerated the next time the custom tool is run.");
            CommentBlock.Add($"   The base file contains the Load(string), Save(string) methods, several constructors,");
            CommentBlock.Add($"   and several properties to work with the class file generated by XSD.exe.");
            CommentBlock.Add($"</auto-generated>");
            CommentBlock.Add($"------------------------------------------------------------------------------");
            CommentBlock.Add($"{NL}");
            return CommentBlock;
        }

        /// <summary>Generates the comment header of the file that says the file was automatically generated and when it will be overwritten.</summary>
        /// <returns>There are 3 Environment.NewLine keys inserted at the end of this comment.</returns>
        protected virtual CodeCommentStatementCollection GetComment_SaveMethod()
        {
            CodeCommentStatementCollection CommentBlock = new CodeCommentStatementCollection();
            CommentBlock.Add($" <summary>");
            CommentBlock.Add($" This method will take the {ParsedFile.TopLevelClass.ClassName} object, create an XML serializer for it, and write the XML to the <paramref name = \"FilePath\" />");
            CommentBlock.Add($" </summary>");
            CommentBlock.Add($" <remarks>This code was generated by XSDCustomTool VisualStudio Extension.</remarks>");
            CommentBlock.Add($" <param name=\"FilePath\"> Destination file path to save the file into. </param>");
            return CommentBlock;
        }

        /// <summary>Generates the comment header of the file that says the file was automatically generated and when it will be overwritten.</summary>
        /// <returns>There are 3 Environment.NewLine keys inserted at the end of this comment.</returns>
        protected virtual CodeCommentStatementCollection GetComment_LoadMethod()
        {
            CodeCommentStatementCollection CommentBlock = new CodeCommentStatementCollection();
            CommentBlock.Add($" <summary>");
            CommentBlock.Add($" Load a file path and produce a Deserialized <typeparamref name=\"{ParsedFile.TopLevelClass.ClassName}\"/> Object");
            CommentBlock.Add($" </summary>");
            CommentBlock.Add($" <remarks>This code was generated by XSDCustomTool VisualStudio Extension.</remarks>");
            CommentBlock.Add($" <param name=\"FilePath\"> This XML file to read into the class object </param>");
            CommentBlock.Add($" <returns> A new <typeparamref name=\"{ParsedFile.TopLevelClass.ClassName}\"/> object </returns>");
            return CommentBlock;
        }

        #endregion </ Comment  Methods >

    }
}
