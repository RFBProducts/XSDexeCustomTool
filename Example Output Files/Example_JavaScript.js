﻿//------------------------------------------------------------------------------
/// <autogenerated>
///     This code was generated by a tool.
///     Runtime Version: 4.0.30319.42000
///
///     Changes to this file may cause incorrect behavior and will be lost if 
///     the code is regenerated.
/// </autogenerated>
//------------------------------------------------------------------------------

//@cc_on
//@set @debug(off)

import System.Xml.Serialization;

//
//This source code was auto-generated by xsd, Version=4.8.3928.0.
//
package XSDCustomToolVSIX.Example_Files {
    
    ///<remarks/>
    public System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0") System.SerializableAttribute() System.Diagnostics.DebuggerStepThroughAttribute() System.ComponentModel.DesignerCategoryAttribute("code") System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true) System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false) 
    class XSDCustomTool_Parameters {
        
        private var xSDexeOptionsField : XSDCustomTool_ParametersXSDexeOptions;
        
        private var elementsToGenerateCodeForField : System.String[];
        
        ///<remarks/>
        public final function get XSDexeOptions() : XSDCustomTool_ParametersXSDexeOptions {
            return this.xSDexeOptionsField;
        }
        public final function set XSDexeOptions(value : XSDCustomTool_ParametersXSDexeOptions) {
            this.xSDexeOptionsField = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlArrayItemAttribute("Element", IsNullable=false) 
        function get ElementsToGenerateCodeFor() : System.String[] {
            return this.elementsToGenerateCodeForField;
        }
        public final function set ElementsToGenerateCodeFor(value : System.String[]) {
            this.elementsToGenerateCodeForField = value;
        }
    }
    
    ///<remarks/>
    public System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0") System.SerializableAttribute() System.Diagnostics.DebuggerStepThroughAttribute() System.ComponentModel.DesignerCategoryAttribute("code") System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true) 
    class XSDCustomTool_ParametersXSDexeOptions {
        
        private var nameSpaceField : System.String;
        
        private var languageField : XSDCustomTool_ParametersXSDexeOptionsLanguage;
        
        private var languageFieldSpecified : boolean;
        
        private var noLogoField : boolean;
        
        private var noLogoFieldSpecified : boolean;
        
        private var generateClassField : boolean;
        
        private var classOptionsField : XSDCustomTool_ParametersXSDexeOptionsClassOptions;
        
        private var dataSetOptionsField : XSDCustomTool_ParametersXSDexeOptionsDataSetOptions;
        
        ///<remarks/>
        public final function get NameSpace() : System.String {
            return this.nameSpaceField;
        }
        public final function set NameSpace(value : System.String) {
            this.nameSpaceField = value;
        }
        
        ///<remarks/>
        public final function get Language() : XSDCustomTool_ParametersXSDexeOptionsLanguage {
            return this.languageField;
        }
        public final function set Language(value : XSDCustomTool_ParametersXSDexeOptionsLanguage) {
            this.languageField = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlIgnoreAttribute() 
        function get LanguageSpecified() : boolean {
            return this.languageFieldSpecified;
        }
        public final function set LanguageSpecified(value : boolean) {
            this.languageFieldSpecified = value;
        }
        
        ///<remarks/>
        public final function get NoLogo() : boolean {
            return this.noLogoField;
        }
        public final function set NoLogo(value : boolean) {
            this.noLogoField = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlIgnoreAttribute() 
        function get NoLogoSpecified() : boolean {
            return this.noLogoFieldSpecified;
        }
        public final function set NoLogoSpecified(value : boolean) {
            this.noLogoFieldSpecified = value;
        }
        
        ///<remarks/>
        public final function get GenerateClass() : boolean {
            return this.generateClassField;
        }
        public final function set GenerateClass(value : boolean) {
            this.generateClassField = value;
        }
        
        ///<remarks/>
        public final function get ClassOptions() : XSDCustomTool_ParametersXSDexeOptionsClassOptions {
            return this.classOptionsField;
        }
        public final function set ClassOptions(value : XSDCustomTool_ParametersXSDexeOptionsClassOptions) {
            this.classOptionsField = value;
        }
        
        ///<remarks/>
        public final function get DataSetOptions() : XSDCustomTool_ParametersXSDexeOptionsDataSetOptions {
            return this.dataSetOptionsField;
        }
        public final function set DataSetOptions(value : XSDCustomTool_ParametersXSDexeOptionsDataSetOptions) {
            this.dataSetOptionsField = value;
        }
    }
    
    ///<remarks/>
    public System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0") System.SerializableAttribute() System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true) 
    enum XSDCustomTool_ParametersXSDexeOptionsLanguage {
        
        ///<remarks/>
        CS,
        
        ///<remarks/>
        VB,
        
        ///<remarks/>
        JS,
        
        ///<remarks/>
        VJS,
    }
    
    ///<remarks/>
    public System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0") System.SerializableAttribute() System.Diagnostics.DebuggerStepThroughAttribute() System.ComponentModel.DesignerCategoryAttribute("code") System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true) 
    class XSDCustomTool_ParametersXSDexeOptionsClassOptions {
        
        private var propertiesInsteadOfFieldsField : boolean;
        
        private var propertiesInsteadOfFieldsFieldSpecified : boolean;
        
        private var orderField : boolean;
        
        private var orderFieldSpecified : boolean;
        
        private var enableDataBindingField : boolean;
        
        private var enableDataBindingFieldSpecified : boolean;
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified) 
        function get PropertiesInsteadOfFields() : boolean {
            return this.propertiesInsteadOfFieldsField;
        }
        public final function set PropertiesInsteadOfFields(value : boolean) {
            this.propertiesInsteadOfFieldsField = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlIgnoreAttribute() 
        function get PropertiesInsteadOfFieldsSpecified() : boolean {
            return this.propertiesInsteadOfFieldsFieldSpecified;
        }
        public final function set PropertiesInsteadOfFieldsSpecified(value : boolean) {
            this.propertiesInsteadOfFieldsFieldSpecified = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified) 
        function get Order() : boolean {
            return this.orderField;
        }
        public final function set Order(value : boolean) {
            this.orderField = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlIgnoreAttribute() 
        function get OrderSpecified() : boolean {
            return this.orderFieldSpecified;
        }
        public final function set OrderSpecified(value : boolean) {
            this.orderFieldSpecified = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified) 
        function get EnableDataBinding() : boolean {
            return this.enableDataBindingField;
        }
        public final function set EnableDataBinding(value : boolean) {
            this.enableDataBindingField = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlIgnoreAttribute() 
        function get EnableDataBindingSpecified() : boolean {
            return this.enableDataBindingFieldSpecified;
        }
        public final function set EnableDataBindingSpecified(value : boolean) {
            this.enableDataBindingFieldSpecified = value;
        }
    }
    
    ///<remarks/>
    public System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0") System.SerializableAttribute() System.Diagnostics.DebuggerStepThroughAttribute() System.ComponentModel.DesignerCategoryAttribute("code") System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true) 
    class XSDCustomTool_ParametersXSDexeOptionsDataSetOptions {
        
        private var enableLinqDataSetField : boolean;
        
        private var enableLinqDataSetFieldSpecified : boolean;
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified) 
        function get EnableLinqDataSet() : boolean {
            return this.enableLinqDataSetField;
        }
        public final function set EnableLinqDataSet(value : boolean) {
            this.enableLinqDataSetField = value;
        }
        
        ///<remarks/>
        public final System.Xml.Serialization.XmlIgnoreAttribute() 
        function get EnableLinqDataSetSpecified() : boolean {
            return this.enableLinqDataSetFieldSpecified;
        }
        public final function set EnableLinqDataSetSpecified(value : boolean) {
            this.enableLinqDataSetFieldSpecified = value;
        }
    }
}