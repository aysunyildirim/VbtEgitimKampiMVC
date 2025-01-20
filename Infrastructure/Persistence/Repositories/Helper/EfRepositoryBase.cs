using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper.Dynamic;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper.Paging;

namespace VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper
{
    public class EfRepositoryBase<TEntity, TEntityId, TContext>
      : IAsyncRepository<TEntity, TEntityId>, IRepository<TEntity, TEntityId>
      where TEntity : Entity<TEntityId>
      where TContext : DbContext
    {
        protected readonly TContext Context;
        private readonly IConfiguration configuration;

        public EfRepositoryBase(TContext context, IConfiguration configuration = null)
        {
            Context = context;
            this.configuration = configuration;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                entity.CreatedDate = DateTime.Now;
            await Context.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return await queryable.AnyAsync(cancellationToken);
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, string? modUser, bool permanent = false)
        {
            await SetEntityAsDeletedAsync(entity, modUser, permanent);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, string? modUser, bool permanent = false)
        {
            await SetEntityAsDeletedAsync(entities, modUser, permanent);
            await Context.SaveChangesAsync();

            return entities;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
        }
        public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = int.MaxValue, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();

            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            if (orderBy != null)
                return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);
            return await queryable.ToPaginateAsync(index, size, cancellationToken);
        }

        public async Task<Paginate<TEntity>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = int.MaxValue, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return await queryable.ToPaginateAsync(index, size, cancellationToken);
        }

        public IQueryable<TEntity> Query() => Context.Set<TEntity>();

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.UpdatedDate = DateTime.Now;
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                entity.UpdatedDate = DateTime.Now;
            Context.UpdateRange(entities);
            await Context.SaveChangesAsync();

            return entities;
        }


        protected async Task SetEntityAsDeletedAsync(TEntity entity, string? modUser, bool permanent)
        {
            if (!permanent)
            {
                CheckHasEntityHaveOneToOneRelation(entity);
                await setEntityAsSoftDeletedAsync(entity, modUser);
            }
            else
            {
                Context.Remove(entity);
            }
        }

        protected void CheckHasEntityHaveOneToOneRelation(TEntity entity)
        {
            try
            {
                bool hasEntityHaveOneToOneRelation =
            Context
                .Entry(entity)
                .Metadata.GetForeignKeys()
                .All(
                    x =>
                        x.DependentToPrincipal?.IsCollection == true
                        || x.PrincipalToDependent?.IsCollection == true
                        || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                ) == false;
                if (hasEntityHaveOneToOneRelation)
                    throw new InvalidOperationException(
                        "Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key."
                    );
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private async Task setEntityAsSoftDeletedAsync(IEntityTimestamps entity, string? modUser)
        {
            if (entity.DeletedDate.HasValue)
                return;

            if (!string.IsNullOrEmpty(modUser))
                entity.ModUser = modUser;

            entity.DeletedDate = DateTime.Now;

            var navigations = Context
                .Entry(entity)
                .Metadata.GetNavigations()
                .Where(x => x is { ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade })
                .ToList();

            foreach (INavigation? navigation in navigations)
            {
                if (navigation.TargetEntityType.IsOwned())
                    continue;
                if (navigation.PropertyInfo == null)
                    continue;

                object? navValue = navigation.PropertyInfo.GetValue(entity);
                if (navigation.IsCollection)
                {
                    if (navValue == null)
                    {
                        IQueryable query = Context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                        navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                        if (navValue == null)
                            continue;
                    }

                    foreach (IEntityTimestamps navValueItem in (IEnumerable)navValue)
                        await setEntityAsSoftDeletedAsync(navValueItem, modUser);
                }
                else
                {
                    if (navValue == null)
                    {
                        IQueryable query = Context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                        navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                            .FirstOrDefaultAsync();
                        if (navValue == null)
                            continue;
                    }

                    await setEntityAsSoftDeletedAsync((IEntityTimestamps)navValue, modUser);
                }
            }

            Context.Update(entity);
        }

        protected IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
        {
            Type queryProviderType = query.Provider.GetType();
            MethodInfo createQueryMethod =
                queryProviderType
                    .GetMethods()
                    .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                    ?.MakeGenericMethod(navigationPropertyType)
                ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
            var queryProviderQuery =
                (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: new object[] { query.Expression })!;
            return queryProviderQuery.Where(x => !((IEntityTimestamps)x).DeletedDate.HasValue);
        }

        protected async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, string? modUser, bool permanent)
        {
            foreach (TEntity entity in entities)
                await SetEntityAsDeletedAsync(entity, modUser, permanent);
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true)
        {
            throw new NotImplementedException();
        }

        public Paginate<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = int.MaxValue, bool withDeleted = false, bool enableTracking = true)
        {
            throw new NotImplementedException();
        }

        public Paginate<TEntity> GetListByDynamic(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = int.MaxValue, bool withDeleted = false, bool enableTracking = true)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true)
        {
            throw new NotImplementedException();
        }

        public TEntity Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public TEntity Delete(TEntity entity, bool permanent = false)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> DeleteRange(ICollection<TEntity> entity, bool permanent = false)
        {
            throw new NotImplementedException();
        }

        #region oldAudit
        //private void AddChangeLogs<TEntity>(TEntity entity, OperationType operationType) where TEntity : class
        //{
        //    if (entity is not ITrackChanges)
        //        return;

        //    // Entity'nin adını al
        //    var entityName = entity.GetType().Name;

        //    // Tablonun mevcut olup olmadığını kontrol et
        //    EnsureChangeLogTableExists(entityName);

        //    // Dinamik tablo ismini oluştur
        //    string tableName = $"{entityName}_ChangeLog";
        //    string schemaName = GetSchemaName(); // Şema adını al

        //    var changeTime = DateTime.Now;
        //    var entry = Context.Entry(entity);

        //    var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString();

        //    // ChangedBy alanını belirle
        //    var changedBy = operationType switch
        //    {
        //        OperationType.Create => (entity as Entity<long>)?.CreUser, // Yeni kayıt, CreUser kullan
        //        OperationType.Update => (entity as Entity<long>)?.ModUser, // Güncelleme, ModUser kullan
        //        OperationType.Delete => (entity as Entity<long>)?.ModUser, // Silme, ModUser kullan
        //        _ => "System" // Varsayılan olarak "System"
        //    };

        //    // İlk seviyedeki property değişikliklerini işle
        //    ProcessEntityChanges(entry, entityId, entityName, schemaName, tableName, changedBy, operationType, changeTime);

        //    // Nested (iç içe) varlıkları işlemek için navigation properties'i kontrol et
        //    foreach (var navigation in entry.Navigations)
        //    {
        //        if (navigation.CurrentValue is IEnumerable<object> collection) // Koleksiyon tipi ise (örneğin liste, dizi)
        //        {
        //            foreach (var nestedEntity in collection)
        //            {
        //                // İç içe varlıkta ITrackChanges implement ediliyor mu kontrol et
        //                if (nestedEntity is ITrackChanges)
        //                {
        //                    var nestedEntityName = nestedEntity.GetType().Name;

        //                    // Nested entity için log tablosu var mı kontrol et
        //                    EnsureChangeLogTableExists(nestedEntityName);

        //                    var nestedEntry = Context.Entry(nestedEntity);
        //                    var nestedEntityId = nestedEntry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString();

        //                    // İç içe varlıklar için değişiklikleri işle
        //                    ProcessEntityChanges(nestedEntry, nestedEntityId, nestedEntityName, schemaName, tableName, changedBy, operationType, changeTime);

        //                }
        //            }
        //        }
        //        else if (navigation.CurrentValue != null) // Tek bir nested entity ise
        //        {
        //            var nestedEntry = Context.Entry(navigation.CurrentValue);
        //            if (nestedEntry.Entity is ITrackChanges)
        //            {
        //                var nestedEntityId = nestedEntry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString();
        //                var nestedEntityName = navigation.CurrentValue.GetType().Name;
        //                var nestedTableName = $"{nestedEntityName}_ChangeLog";

        //                // Nested entity için log tablosu var mı kontrol et
        //                EnsureChangeLogTableExists(nestedEntityName);

        //                // İç içe varlıklar için değişiklikleri işle
        //                ProcessEntityChanges(nestedEntry, nestedEntityId, nestedEntityName, schemaName, nestedTableName, changedBy, operationType, changeTime);

        //            }
        //        }
        //    }
        //}

        //private void ProcessEntityChanges(EntityEntry entry, string entityId, string entityName, string schemaName, string tableName, string changedBy, OperationType operationType, DateTime changeTime)
        //{
        //    // Sınıf seviyesinde TrackAllProperties attribute'u kontrol et
        //    bool trackAllProperties = Attribute.IsDefined(entry.Entity.GetType(), typeof(TrackAllPropertiesAttribute));

        //    foreach (var prop in entry.Properties)
        //    {
        //        // Track edilecek property'leri belirle
        //        if (ShouldTrackProperty(prop, trackAllProperties))
        //        {
        //            if (ShouldLogChange(prop, operationType))
        //                LogPropertyChange(prop, entityId, entityName, schemaName, tableName, changedBy, operationType, changeTime);
        //        }
        //    }
        //}

        //private bool ShouldTrackProperty(PropertyEntry prop, bool trackAllProperties)
        //{
        //    // Eğer tüm property'ler track edilecekse veya property 'TrackChangesAttribute' ile işaretlenmişse true döner
        //    return trackAllProperties || Attribute.IsDefined(prop.Metadata.PropertyInfo, typeof(TrackChangesAttribute));
        //}

        //private bool ShouldLogChange(PropertyEntry prop, OperationType operationType)
        //{
        //    // Update işleminde sadece değişen property'ler loglanır
        //    if (operationType == OperationType.Update)
        //        return prop.IsModified && !Equals(prop.OriginalValue, prop.CurrentValue);

        //    // Create ve Delete işlemlerinde tüm property'ler loglanır
        //    return operationType == OperationType.Create || operationType == OperationType.Delete;
        //}

        //private void LogPropertyChange(PropertyEntry prop, string entityId, string entityName, string schemaName, string tableName, string changedBy, OperationType operationType, DateTime changeTime)
        //{
        //    //if (prop == null)
        //    //    throw new ArgumentNullException(nameof(prop), "Property entry cannot be null.");

        //    // Değişiklik logunu oluştur
        //    var changeLog = new ChangeLog
        //    {
        //        EntityId = Convert.ToInt64(entityId),
        //        PropertyName = prop.Metadata.Name,
        //        OldValue = prop.OriginalValue?.ToString() ?? "NULL",
        //        NewValue = prop.CurrentValue?.ToString() ?? "NULL",
        //        ChangeDate = changeTime,
        //        ChangedBy = string.IsNullOrEmpty(changedBy) ? "System" : changedBy,
        //        OperationType = operationType
        //    };

        //    // Dinamik tabloya log ekle
        //    InsertChangeLog(changeLog, schemaName, tableName);
        //}
        //private void InsertChangeLog(ChangeLog changeLog, string schemaName, string tableName)
        //{

        //    // SQL sorgusunu bir değişkene atıyoruz
        //    var query = $@"
        //            INSERT INTO {schemaName}.""{tableName}"" 
        //            (""EntityId"", ""PropertyName"", ""OldValue"", ""NewValue"", ""ChangeDate"", ""ChangedBy"", ""OperationType"",""CreatedDate"")
        //            VALUES (:EntityId, :PropertyName, :OldValue, :NewValue, :ChangeDate, :ChangedBy, :OperationType, :CreatedDate)";

        //    // Daha sonra bu değişkeni ExecuteSqlRaw içinde kullanıyoruz
        //    Context.Database.ExecuteSqlRaw(query,
        //        new OracleParameter("EntityId", OracleDbType.Int64, changeLog.EntityId, System.Data.ParameterDirection.Input),
        //        new OracleParameter("PropertyName", OracleDbType.Varchar2, changeLog.PropertyName ?? (object)DBNull.Value, System.Data.ParameterDirection.Input),
        //        new OracleParameter("OldValue", OracleDbType.Varchar2, changeLog.OldValue ?? (object)DBNull.Value, System.Data.ParameterDirection.Input),
        //        new OracleParameter("NewValue", OracleDbType.Varchar2, changeLog.NewValue ?? (object)DBNull.Value, System.Data.ParameterDirection.Input),
        //        new OracleParameter("ChangeDate", OracleDbType.TimeStamp, changeLog.ChangeDate, System.Data.ParameterDirection.Input),
        //        new OracleParameter("ChangedBy", OracleDbType.Varchar2, changeLog.ChangedBy ?? (object)DBNull.Value, System.Data.ParameterDirection.Input),
        //        new OracleParameter("OperationType", OracleDbType.Int32, (int)changeLog.OperationType, System.Data.ParameterDirection.Input),
        //        new OracleParameter("CreatedDate", OracleDbType.TimeStamp, DateTime.Now, System.Data.ParameterDirection.Input)
        //    );

        //}
        //private void EnsureChangeLogTableExists(string entityName)
        //{

        //    string tableName = $"{entityName}_ChangeLog";
        //    string schemaName = GetSchemaName();

        //    var sql = $@"
        //            SELECT 
        //                CASE 
        //                    WHEN COUNT(*) > 0 THEN 1
        //                    ELSE 0
        //                END AS TableExists
        //            FROM all_tables
        //            WHERE table_name = '{tableName}s' 
        //            AND owner = '{schemaName}'";

        //    int tableExists;

        //    using (var command = Context.Database.GetDbConnection().CreateCommand())
        //    {
        //        command.CommandText = sql;
        //        Context.Database.OpenConnection();
        //        tableExists = Convert.ToInt32(command.ExecuteScalar() ?? 0);
        //    }

        //    if (tableExists == 0)
        //        throw new Exception($"Tablo bulunamadı: {schemaName}.{tableName}");


        //}


        //private string GetSchemaName()
        //{
        //    return configuration.GetValue<string>("ConnectionStrings:DefaultSchema");
        //}
        #endregion


        // IRawSqlQueryService'in Implementasyonu
        public async Task<List<T>> ExecuteQueryAsync<T>(string sqlQuery, object[] parameters = null) where T : class, new()
        {
            parameters ??= Array.Empty<object>();

            return await Context.Set<T>()
                .FromSqlRaw(sqlQuery, parameters)
                .ToListAsync();
        }

        public async Task<int> ExecuteCommandAsync(string sqlCommand, object[] parameters = null)
        {
            parameters ??= Array.Empty<object>();

            return await Context.Database.ExecuteSqlRawAsync(sqlCommand, parameters);
        }

        public async Task<List<dynamic>> ExecuteQueryAsync(string sqlQuery, object[] parameters = null)
        {
            try
            {
                parameters ??= Array.Empty<object>();

                using var command = Context.Database.GetDbConnection().CreateCommand();
                command.CommandText = sqlQuery;
                command.CommandType = System.Data.CommandType.Text;

                if (parameters is not null && parameters.Length > 0)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                if (command.Connection.State != System.Data.ConnectionState.Open)
                {
                    await command.Connection.OpenAsync();
                }

                using var reader = await command.ExecuteReaderAsync();
                var results = new List<dynamic>();

                while (await reader.ReadAsync())
                {
                    var row = new ExpandoObject() as IDictionary<string, object>;
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                    results.Add(row);
                }

                return results;
            }
            catch (Exception ex)
            {
                // Hataları loglayabilir veya özel bir işlem yapabilirsiniz.
                throw new InvalidOperationException($"SQL sorgusu çalıştırılırken bir hata oluştu: {ex.Message}", ex);
            }
        }

    }
}
