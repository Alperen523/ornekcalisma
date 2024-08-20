using System.Collections.Generic;

namespace UzmanCrm.CrmService.DAL.Config.Application.CRM.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AssociatedMenuConfiguration
    {
        public int? Behavior { get; set; }
        public int? Group { get; set; }
        public Label Label { get; set; }
        public int? Order { get; set; }
    }

    public class Attribute
    {
        public int? Format { get; set; } = null;
        public int? ImeMode { get; set; } = null;
        public string AttributeOf { get; set; }
        public int? AttributeType { get; set; } = null;
        public int? ColumnNumber { get; set; } = null;
        public Description Description { get; set; }
        public DisplayName DisplayName { get; set; }
        public object DeprecatedVersion { get; set; }
        public string EntityLogicalName { get; set; }
        public IsAuditEnabled IsAuditEnabled { get; set; }
        public bool IsCustomAttribute { get; set; }
        public bool IsPrimaryId { get; set; }
        public bool IsPrimaryName { get; set; }
        public bool IsValidForCreate { get; set; }
        public bool IsValidForRead { get; set; }
        public bool IsValidForUpdate { get; set; }
        public bool CanBeSecuredForRead { get; set; }
        public bool CanBeSecuredForCreate { get; set; }
        public bool CanBeSecuredForUpdate { get; set; }
        public bool IsSecured { get; set; }
        public bool IsRetrievable { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSearchable { get; set; }
        public bool IsManaged { get; set; }
        public IsGlobalFilterEnabled IsGlobalFilterEnabled { get; set; }
        public IsSortableEnabled IsSortableEnabled { get; set; }
        public object LinkedAttributeId { get; set; }
        public string LogicalName { get; set; }
        public IsCustomizable IsCustomizable { get; set; }
        public IsRenameable IsRenameable { get; set; }
        public IsValidForAdvancedFind IsValidForAdvancedFind { get; set; }
        public RequiredLevel RequiredLevel { get; set; }
        public CanModifyAdditionalSettings CanModifyAdditionalSettings { get; set; }
        public string SchemaName { get; set; }
        public object InheritsFrom { get; set; }
        public string MetadataId { get; set; }
        public AttributeTypeName AttributeTypeName { get; set; }
        public string IntroducedVersion { get; set; }
        public object HasChanged { get; set; }
        public int? SourceTypeMask { get; set; } = null;
        public string FormulaDefinition { get; set; }
        public bool IsLogical { get; set; }
        public int? SourceType { get; set; } = null;
        public DateTimeBehavior DateTimeBehavior { get; set; }
        public CanChangeDateTimeBehavior CanChangeDateTimeBehavior { get; set; }
        public int? DefaultFormValue { get; set; } = null;
        public OptionSet OptionSet { get; set; }
        public List<string> Targets { get; set; }
        public int? MaxLength { get; set; }
        public string YomiOf { get; set; }
        public FormatName FormatName { get; set; }
        public bool? IsLocalizable { get; set; }
        public double? MaxValue { get; set; }
        public double? MinValue { get; set; } = 0;
    }

    public class AttributeTypeName
    {
        public string Value { get; set; }
    }

    public class CanBeInManyToMany
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanBePrimaryEntityInRelationship
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanBeRelatedEntityInRelationship
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanChangeDateTimeBehavior
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanChangeHierarchicalRelationship
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanCreateAttributes
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanCreateCharts
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanCreateForms
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanCreateViews
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanEnableSyncToExternalSearchIndex
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CanModifyAdditionalSettings
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class CascadeConfiguration
    {
        public int Assign { get; set; }
        public int Delete { get; set; }
        public int Merge { get; set; }
        public int Reparent { get; set; }
        public int Share { get; set; }
        public int Unshare { get; set; }
        public int? RollupView { get; set; }
    }

    public class DateTimeBehavior
    {
        public string Value { get; set; }
    }

    public class Description
    {
        public List<LocalizedLabel> LocalizedLabels { get; set; }
        public UserLocalizedLabel UserLocalizedLabel { get; set; }
    }

    public class DisplayCollectionName
    {
        public List<LocalizedLabel> LocalizedLabels { get; set; }
        public UserLocalizedLabel UserLocalizedLabel { get; set; }
    }

    public class DisplayName
    {
        public List<LocalizedLabel> LocalizedLabels { get; set; }
        public UserLocalizedLabel UserLocalizedLabel { get; set; }
    }

    public class FormatName
    {
        public string Value { get; set; }
    }

    public class IsAuditEnabled
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsConnectionsEnabled
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsCustomizable
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsDuplicateDetectionEnabled
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsGlobalFilterEnabled
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsMailMergeEnabled
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsMappable
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsOfflineInMobileClient
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsReadOnlyInMobileClient
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsRenameable
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsSortableEnabled
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsValidForAdvancedFind
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsValidForQueue
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsVisibleInMobile
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class IsVisibleInMobileClient
    {
        public bool Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class Label
    {
        public List<LocalizedLabel> LocalizedLabels { get; set; }
        public UserLocalizedLabel UserLocalizedLabel { get; set; }
    }

    public class LocalizedLabel
    {
        public string Label { get; set; }
        public int LanguageCode { get; set; }
        public bool IsManaged { get; set; }
        public object MetadataId { get; set; }
        public object HasChanged { get; set; }
    }

    public class ManyToOneRelationship
    {
        public AssociatedMenuConfiguration AssociatedMenuConfiguration { get; set; }
        public CascadeConfiguration CascadeConfiguration { get; set; }
        public string ReferencedAttribute { get; set; }
        public string ReferencedEntity { get; set; }
        public string ReferencingAttribute { get; set; }
        public string ReferencingEntity { get; set; }
        public bool IsHierarchical { get; set; }
        public object ReferencedEntityNavigationPropertyName { get; set; }
        public object ReferencingEntityNavigationPropertyName { get; set; }
        public bool IsCustomRelationship { get; set; }
        public IsCustomizable IsCustomizable { get; set; }
        public bool IsValidForAdvancedFind { get; set; }
        public string SchemaName { get; set; }
        public int SecurityTypes { get; set; }
        public bool IsManaged { get; set; }
        public string MetadataId { get; set; }
        public int RelationshipType { get; set; }
        public object IntroducedVersion { get; set; }
        public object HasChanged { get; set; }
    }

    public class OneToManyRelationship
    {
        public AssociatedMenuConfiguration AssociatedMenuConfiguration { get; set; }
        public CascadeConfiguration CascadeConfiguration { get; set; }
        public string ReferencedAttribute { get; set; }
        public string ReferencedEntity { get; set; }
        public string ReferencingAttribute { get; set; }
        public string ReferencingEntity { get; set; }
        public bool IsHierarchical { get; set; }
        public object ReferencedEntityNavigationPropertyName { get; set; }
        public object ReferencingEntityNavigationPropertyName { get; set; }
        public bool IsCustomRelationship { get; set; }
        public IsCustomizable IsCustomizable { get; set; }
        public bool IsValidForAdvancedFind { get; set; }
        public string SchemaName { get; set; }
        public int SecurityTypes { get; set; }
        public bool IsManaged { get; set; }
        public string MetadataId { get; set; }
        public int RelationshipType { get; set; }
        public object IntroducedVersion { get; set; }
        public object HasChanged { get; set; }
    }

    public class Option
    {
        public int DefaultStatus { get; set; }
        public string InvariantName { get; set; }
        public int Value { get; set; }
        public Label Label { get; set; }
        public Description Description { get; set; }
        public object Color { get; set; }
        public bool IsManaged { get; set; }
        public object MetadataId { get; set; }
        public object HasChanged { get; set; }
        public int? State { get; set; }
        public string TransitionData { get; set; }
    }

    public class OptionSet
    {
        public List<Option> Options { get; set; }
        public Description Description { get; set; }
        public DisplayName DisplayName { get; set; }
        public bool IsCustomOptionSet { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsManaged { get; set; }
        public IsCustomizable IsCustomizable { get; set; }
        public string Name { get; set; }
        public int OptionSetType { get; set; }
        public string MetadataId { get; set; }
        public string IntroducedVersion { get; set; }
        public object HasChanged { get; set; }
    }

    public class Privilege
    {
        public bool CanBeBasic { get; set; }
        public bool CanBeDeep { get; set; }
        public bool CanBeGlobal { get; set; }
        public bool CanBeLocal { get; set; }
        public bool CanBeEntityReference { get; set; }
        public bool CanBeParentEntityReference { get; set; }
        public string Name { get; set; }
        public string PrivilegeId { get; set; }
        public int PrivilegeType { get; set; }
    }

    public class RequiredLevel
    {
        public int Value { get; set; }
        public bool CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
    }

    public class EntityMetadata_Temp
    {
        public int ActivityTypeMask { get; set; }
        public List<Attribute> Attributes { get; set; }
        public bool AutoRouteToOwnerQueue { get; set; }
        public bool CanTriggerWorkflow { get; set; }
        public Description Description { get; set; }
        public DisplayCollectionName DisplayCollectionName { get; set; }
        public DisplayName DisplayName { get; set; }
        public bool IsDocumentManagementEnabled { get; set; }
        public bool IsOneNoteIntegrationEnabled { get; set; }
        public bool IsInteractionCentricEnabled { get; set; }
        public bool IsKnowledgeManagementEnabled { get; set; }
        public bool IsDocumentRecommendationsEnabled { get; set; }
        public object AutoCreateAccessTeams { get; set; }
        public bool IsActivity { get; set; }
        public bool IsActivityParty { get; set; }
        public IsAuditEnabled IsAuditEnabled { get; set; }
        public bool IsAvailableOffline { get; set; }
        public bool IsChildEntity { get; set; }
        public bool IsAIRUpdated { get; set; }
        public IsValidForQueue IsValidForQueue { get; set; }
        public IsConnectionsEnabled IsConnectionsEnabled { get; set; }
        public object IconLargeName { get; set; }
        public string IconMediumName { get; set; }
        public string IconSmallName { get; set; }
        public bool IsCustomEntity { get; set; }
        public bool IsBusinessProcessEnabled { get; set; }
        public IsCustomizable IsCustomizable { get; set; }
        public IsRenameable IsRenameable { get; set; }
        public IsMappable IsMappable { get; set; }
        public IsDuplicateDetectionEnabled IsDuplicateDetectionEnabled { get; set; }
        public CanCreateAttributes CanCreateAttributes { get; set; }
        public CanCreateForms CanCreateForms { get; set; }
        public CanCreateViews CanCreateViews { get; set; }
        public CanCreateCharts CanCreateCharts { get; set; }
        public CanBeRelatedEntityInRelationship CanBeRelatedEntityInRelationship { get; set; }
        public CanBePrimaryEntityInRelationship CanBePrimaryEntityInRelationship { get; set; }
        public CanBeInManyToMany CanBeInManyToMany { get; set; }
        public CanEnableSyncToExternalSearchIndex CanEnableSyncToExternalSearchIndex { get; set; }
        public bool SyncToExternalSearchIndex { get; set; }
        public CanModifyAdditionalSettings CanModifyAdditionalSettings { get; set; }
        public bool ChangeTrackingEnabled { get; set; }
        public object CanChangeTrackingBeEnabled { get; set; }
        public bool IsImportable { get; set; }
        public bool IsIntersect { get; set; }
        public IsMailMergeEnabled IsMailMergeEnabled { get; set; }
        public bool IsManaged { get; set; }
        public bool IsEnabledForCharts { get; set; }
        public bool IsEnabledForTrace { get; set; }
        public bool IsValidForAdvancedFind { get; set; }
        public IsVisibleInMobile IsVisibleInMobile { get; set; }
        public IsVisibleInMobileClient IsVisibleInMobileClient { get; set; }
        public IsReadOnlyInMobileClient IsReadOnlyInMobileClient { get; set; }
        public bool IsReadingPaneEnabled { get; set; }
        public bool IsQuickCreateEnabled { get; set; }
        public string LogicalName { get; set; }
        public List<object> ManyToManyRelationships { get; set; }
        public List<ManyToOneRelationship> ManyToOneRelationships { get; set; }
        public List<OneToManyRelationship> OneToManyRelationships { get; set; }
        public int ObjectTypeCode { get; set; }
        public int OwnershipType { get; set; }
        public string PrimaryNameAttribute { get; set; }
        public string PrimaryIdAttribute { get; set; }
        public List<Privilege> Privileges { get; set; }
        public object RecurrenceBaseEntityLogicalName { get; set; }
        public string ReportViewName { get; set; }
        public string SchemaName { get; set; }
        public bool IsStateModelAware { get; set; }
        public bool EnforceStateTransitions { get; set; }
        public object UsesBusinessDataLabelTable { get; set; }
        public string MetadataId { get; set; }
        public object PrimaryImageAttribute { get; set; }
        public string IntroducedVersion { get; set; }
        public object HasChanged { get; set; }
        public bool EntityHelpUrlEnabled { get; set; }
        public string EntityHelpUrl { get; set; }
        public CanChangeHierarchicalRelationship CanChangeHierarchicalRelationship { get; set; }
        public bool IsOptimisticConcurrencyEnabled { get; set; }
        public string EntityColor { get; set; }
        public List<object> Keys { get; set; }
        public object LogicalCollectionName { get; set; }
        public object CollectionSchemaName { get; set; }
        public IsOfflineInMobileClient IsOfflineInMobileClient { get; set; }
        public int DaysSinceRecordLastModified { get; set; }
        public string EntitySetName { get; set; }
        public bool IsEnabledForExternalChannels { get; set; }
        public object IsPrivate { get; set; }
        public object IsSLAEnabled { get; set; }
        public string MobileOfflineFilters { get; set; }
        public object IsBPFEntity { get; set; }
        public bool IsLogicalEntity { get; set; }
    }

    public class UserLocalizedLabel
    {
        public string Label { get; set; }
        public int LanguageCode { get; set; }
        public bool IsManaged { get; set; }
        public object MetadataId { get; set; }
        public object HasChanged { get; set; }
    }


}
