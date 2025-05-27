using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDNAT_Repository.Entities;

namespace BDNAT_Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly DnaTestingDbContext _context;
        public static GenericRepository<T> _instance;

        public GenericRepository()
        {
            _context = new DnaTestingDbContext();
        }

        public static GenericRepository<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GenericRepository<T>();
                }
                return _instance;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async Task<bool> InsertAsync(T entity)
        {
            try
            {
                await _context.AddAsync<T>(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                _context.ChangeTracker.Clear();

                return false; // Trả về false thay vì throw lỗi
            }
        }
        public async Task<bool> InsertListAsync(List<T> entities)
        {
            try
            {
                await _context.AddRangeAsync(entities); // Thêm nhiều bản ghi cùng lúc
                return await _context.SaveChangesAsync() > 0; // Lưu thay đổi
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                _context.ChangeTracker.Clear();
                throw;
            }
        }


        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                // Tìm tên khóa chính của entity
                var keyName = _context.Model.FindEntityType(typeof(T))
                       .FindPrimaryKey().Properties
                       .Select(x => x.Name)
                       .FirstOrDefault();

                if (keyName == null)
                {
                    throw new Exception("Can not found entity Primary Key.");
                }

                // Lấy giá trị khóa chính
                var keyValue = typeof(T).GetProperty(keyName).GetValue(entity);

                // Kiểm tra xem entity đã tồn tại trong DbContext chưa
                var existingEntry = _context.ChangeTracker.Entries<T>()
                    .FirstOrDefault(e => e.Property(keyName).CurrentValue.Equals(keyValue));

                if (existingEntry != null)
                {
                    // Nếu đã tồn tại trong ChangeTracker, hãy tách nó ra
                    existingEntry.State = EntityState.Detached;
                }

                // Kiểm tra xem entity có tồn tại trong database không
                var existingEntity = await _context.Set<T>().FindAsync(keyValue);
                if (existingEntity != null)
                {
                    // Cập nhật giá trị từ entity mới sang entity đã tồn tại
                    _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    _context.Entry(existingEntity).State = EntityState.Modified;
                }
                else
                {
                    // Nếu không tồn tại, thêm mới entity
                    _context.Set<T>().Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var entityID = await GetById(id);

                if (entityID != null)
                {
                    _context.Remove<T>(entityID);
                    return await _context.SaveChangesAsync() > 0;
                }
                else
                {
                    Console.WriteLine("Not found for deletion");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entityID = await GetById(id);

                if (entityID != null)
                {
                    _context.Remove<T>(entityID);
                    return await _context.SaveChangesAsync() > 0;
                }
                else
                {
                    Console.WriteLine("Not found for deletion");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting: " + ex.Message);
                return false;
            }
        }

        public async Task<T> GetById(string id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async Task<T> GetById(int id)
        {
            return await _context.FindAsync<T>(id);
        }

        public void Detach(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Modified;
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
