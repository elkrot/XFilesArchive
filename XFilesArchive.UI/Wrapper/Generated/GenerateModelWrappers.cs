﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Версия среды выполнения: 15.0.0.0
//  
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторного создания кода.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace XFilesArchive.UI.Wrapper.Generated
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Collections;
    using XFilesArchive.Model;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class GenerateModelWrappers : GenerateModelWrappersBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            
            #line 12 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"

  foreach (var modelType in GetModelTypes())
  {
    var simpleProperties = modelType.GetProperties()
          .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string)).ToList();

    var complexProperties = modelType.GetProperties().Except(simpleProperties)
          .Where(p => p.PropertyType.IsClass 
	                  && !typeof(IEnumerable).IsAssignableFrom(p.PropertyType)).ToList();

    var collectionProperties = modelType.GetProperties()
	      .Except(simpleProperties)
		  .Except(complexProperties)
	      .Where(p=>p.PropertyType.IsGenericType).ToList();


            
            #line default
            #line hidden
            this.Write("using System;   \r\nusing System.Linq;\r\nusing HomeArchiveX.Model;\r\n\r\nnamespace Home" +
                    "ArchiveX.WpfUI.Wrapper\r\n{\r\n  public partial class ");
            
            #line 33 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(modelType.Name));
            
            #line default
            #line hidden
            this.Write("Wrapper : ModelWrapper<");
            
            #line 33 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(modelType.Name));
            
            #line default
            #line hidden
            this.Write(">\r\n  {\r\n    public ");
            
            #line 35 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(modelType.Name));
            
            #line default
            #line hidden
            this.Write("Wrapper(");
            
            #line 35 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(modelType.Name));
            
            #line default
            #line hidden
            this.Write(" model) : base(model)\r\n    {\r\n    }\r\n");
            
            #line 38 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"

	GenerateSimpleProperties(simpleProperties);
	GenerateComplexProperties(complexProperties);
	GenerateCollectionProperties(collectionProperties);
    GenerateInitializeComplexProperties(modelType.Name, complexProperties);
	GenerateInitializeCollectionProperties(modelType.Name, collectionProperties);

            
            #line default
            #line hidden
            this.Write("  }\r\n}\r\n");
            
            #line 47 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
  
  SaveGeneratedCodeAsFile(modelType.Name + "Wrapper.g.cs");
  }

            
            #line default
            #line hidden
            this.Write("\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 52 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
 
  private IEnumerable<Type> GetModelTypes()
  {
	return typeof(ArchiveEntity).Assembly.GetTypes();
  }

  private void SaveGeneratedCodeAsFile(string fileName)
  {
    string dir = Path.GetDirectoryName(Host.TemplateFile);
  
    string outputFilePath = Path.Combine(dir, fileName);
    File.WriteAllText(outputFilePath, GenerationEnvironment.ToString()); 
 
    GenerationEnvironment.Clear();
  }

  private string GetTypeName(Type type)
  {
    if (type.IsGenericType)
    {
      var genericArguments = type.GetGenericArguments().Select(t => GetTypeName(t)).ToArray();
      var typeDefinition = type.GetGenericTypeDefinition().FullName;
      typeDefinition = typeDefinition.Substring(0, typeDefinition.IndexOf('`'));
      return string.Format("{0}<{1}>", typeDefinition, string.Join(",", genericArguments));
    }
    else
    {
      return type.FullName;
    }
  }

  private void GenerateSimpleProperties(IEnumerable<PropertyInfo> properties)
  {
    foreach (var property in properties)
    {
      var propertyType = GetTypeName(property.PropertyType);
      var propertyName= property.Name;

        
        #line default
        #line hidden
        
        #line 89 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("\r\n    public ");

        
        #line default
        #line hidden
        
        #line 91 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyType));

        
        #line default
        #line hidden
        
        #line 91 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" ");

        
        #line default
        #line hidden
        
        #line 91 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 91 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("\r\n    {\r\n      get { return GetValue<");

        
        #line default
        #line hidden
        
        #line 93 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyType));

        
        #line default
        #line hidden
        
        #line 93 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(">(); }\r\n      set { SetValue(value); }\r\n    }\r\n\r\n    public ");

        
        #line default
        #line hidden
        
        #line 97 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyType));

        
        #line default
        #line hidden
        
        #line 97 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" ");

        
        #line default
        #line hidden
        
        #line 97 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 97 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("OriginalValue => GetOriginalValue<");

        
        #line default
        #line hidden
        
        #line 97 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyType));

        
        #line default
        #line hidden
        
        #line 97 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(">(nameof(");

        
        #line default
        #line hidden
        
        #line 97 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 97 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("));\r\n\r\n    public bool ");

        
        #line default
        #line hidden
        
        #line 99 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 99 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("IsChanged => GetIsChanged(nameof(");

        
        #line default
        #line hidden
        
        #line 99 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 99 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("));\r\n");

        
        #line default
        #line hidden
        
        #line 100 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
 
    }
  }

  private void GenerateComplexProperties(IEnumerable<PropertyInfo> properties)
  {
    foreach (var property in properties)
    {
      var propertyType = property.PropertyType.Name;
      var propertyName= property.Name;

        
        #line default
        #line hidden
        
        #line 110 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" \r\n    public ");

        
        #line default
        #line hidden
        
        #line 112 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyType));

        
        #line default
        #line hidden
        
        #line 112 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("Wrapper ");

        
        #line default
        #line hidden
        
        #line 112 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 112 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" { get; private set; }\r\n");

        
        #line default
        #line hidden
        
        #line 113 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
   
    }
  }

  private void GenerateCollectionProperties(IEnumerable<PropertyInfo> properties)
  {
    foreach (var property in properties)
    {
      var itemType = property.PropertyType.GenericTypeArguments[0].Name;
      var propertyName= property.Name;

        
        #line default
        #line hidden
        
        #line 123 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" \r\n    public ChangeTrackingCollection<");

        
        #line default
        #line hidden
        
        #line 125 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(itemType));

        
        #line default
        #line hidden
        
        #line 125 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("Wrapper> ");

        
        #line default
        #line hidden
        
        #line 125 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 125 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" { get; private set; }\r\n");

        
        #line default
        #line hidden
        
        #line 126 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
  
    }
  }

  private void GenerateInitializeComplexProperties(string modelTypeName, IEnumerable<PropertyInfo> properties)
  {
    if(properties.Any())
    {

        
        #line default
        #line hidden
        
        #line 134 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("    \r\n    protected override void InitializeComplexProperties(");

        
        #line default
        #line hidden
        
        #line 136 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(modelTypeName));

        
        #line default
        #line hidden
        
        #line 136 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" model)\r\n    {\r\n");

        
        #line default
        #line hidden
        
        #line 138 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"

	  foreach (var complexProperty in properties)
      {
		var propertyName = complexProperty.Name;
		var propertyType = complexProperty.PropertyType.Name;

        
        #line default
        #line hidden
        
        #line 143 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("      if (model.");

        
        #line default
        #line hidden
        
        #line 144 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 144 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" == null)\r\n      {\r\n        throw new ArgumentException(\"");

        
        #line default
        #line hidden
        
        #line 146 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 146 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" cannot be null\");\r\n      }\r\n      ");

        
        #line default
        #line hidden
        
        #line 148 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 148 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" = new ");

        
        #line default
        #line hidden
        
        #line 148 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyType));

        
        #line default
        #line hidden
        
        #line 148 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("Wrapper(model.");

        
        #line default
        #line hidden
        
        #line 148 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 148 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(");\r\n      RegisterComplex(");

        
        #line default
        #line hidden
        
        #line 149 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 149 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(");\r\n");

        
        #line default
        #line hidden
        
        #line 150 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"

      }

        
        #line default
        #line hidden
        
        #line 152 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("    }\r\n");

        
        #line default
        #line hidden
        
        #line 154 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"

    }
  }

  private void GenerateInitializeCollectionProperties(string modelTypeName, IEnumerable<System.Reflection.PropertyInfo> properties)
  {
    if(properties.Any())
    {

        
        #line default
        #line hidden
        
        #line 162 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("\r\n    protected override void InitializeCollectionProperties(");

        
        #line default
        #line hidden
        
        #line 164 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(modelTypeName));

        
        #line default
        #line hidden
        
        #line 164 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" model)\r\n    {\r\n");

        
        #line default
        #line hidden
        
        #line 166 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"

      foreach(var property in properties)
      {
		var itemType = property.PropertyType.GenericTypeArguments[0].Name;
		var propertyName = property.Name;
		   

        
        #line default
        #line hidden
        
        #line 172 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("      if (model.");

        
        #line default
        #line hidden
        
        #line 173 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 173 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" == null)\r\n      {\r\n        throw new ArgumentException(\"");

        
        #line default
        #line hidden
        
        #line 175 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 175 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" cannot be null\");\r\n      }\r\n \r\n      ");

        
        #line default
        #line hidden
        
        #line 178 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 178 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(" = new ChangeTrackingCollection<");

        
        #line default
        #line hidden
        
        #line 178 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(itemType));

        
        #line default
        #line hidden
        
        #line 178 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("Wrapper>(\r\n        model.");

        
        #line default
        #line hidden
        
        #line 179 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 179 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(".Select(e => new ");

        
        #line default
        #line hidden
        
        #line 179 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(itemType));

        
        #line default
        #line hidden
        
        #line 179 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("Wrapper(e)));\r\n      RegisterCollection(");

        
        #line default
        #line hidden
        
        #line 180 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 180 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(", model.");

        
        #line default
        #line hidden
        
        #line 180 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propertyName));

        
        #line default
        #line hidden
        
        #line 180 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write(");\r\n");

        
        #line default
        #line hidden
        
        #line 181 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"

     }

        
        #line default
        #line hidden
        
        #line 183 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"
this.Write("    }\r\n");

        
        #line default
        #line hidden
        
        #line 185 "D:\GitRepositories\XFilesArchive\XFilesArchive.UI\Wrapper\Generated\GenerateModelWrappers.tt"

    }
  }


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public class GenerateModelWrappersBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}