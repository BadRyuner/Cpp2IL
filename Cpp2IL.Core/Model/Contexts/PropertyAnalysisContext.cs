using Cpp2IL.Core.Utils;
using LibCpp2IL.Metadata;
using StableNameDotNet.Providers;

namespace Cpp2IL.Core.Model.Contexts;

public class PropertyAnalysisContext : HasCustomAttributesAndName, IPropertyInfoProvider
{
    public TypeAnalysisContext DeclaringType { get; }
    public Il2CppPropertyDefinition Definition { get; }

    public MethodAnalysisContext? Getter { get; }
    public MethodAnalysisContext? Setter { get; }

    protected override int CustomAttributeIndex => Definition.customAttributeIndex;

    public override AssemblyAnalysisContext CustomAttributeAssembly => DeclaringType.DeclaringAssembly;

    public override string DefaultName => Definition.Name!;

    public TypeAnalysisContext PropertyTypeContext => DeclaringType.DeclaringAssembly.ResolveIl2CppType(Definition.RawPropertyType!);

    public PropertyAnalysisContext(Il2CppPropertyDefinition definition, TypeAnalysisContext parent) : base(definition.token, parent.AppContext)
    {
        DeclaringType = parent;
        Definition = definition;

        InitCustomAttributeData();

        Getter = parent.GetMethod(definition.Getter);
        Setter = parent.GetMethod(definition.Setter);
    }

    public override string ToString() => $"Property:  {Definition.DeclaringType!.Name}::{Definition.Name}";

    #region StableNameDotNet implementation

    public ITypeInfoProvider PropertyTypeInfoProvider
        => Definition.RawPropertyType!.ThisOrElementIsGenericParam()
            ? new GenericParameterTypeInfoProviderWrapper(Definition.RawPropertyType!.GetGenericParamName())
            : TypeAnalysisContext.GetSndnProviderForType(AppContext, Definition.RawPropertyType!);

    public string PropertyName => Name;

    #endregion
}
