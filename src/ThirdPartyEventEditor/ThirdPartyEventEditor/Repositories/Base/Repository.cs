using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Repositories.Base
{
    internal abstract class Repository<T>
        where T : class, IBaseModel
    {
        private readonly string _filePath;
        private static readonly object _locker = new object();

        protected Repository(string path)
        {
            _filePath = path;
        }


        protected IEnumerable<T> GetAll()
        {
            var items = GetItems();

            return items ?? Enumerable.Empty<T>();
        }


        protected T GetById(int id)
        {
            var items = GetItems();

            var result = items
                .FirstOrDefault(j => j.Id == id);

            return result;
        }

        protected bool Insert(T item)
        {
            const int firstId = 1;

            lock (_locker)
            {
                var items = GetItems();

                if (IsExist(item, items))
                {
                    return false;
                }

                item.Id = items.Any() ? SetNewId(items) : firstId;

                items.Add(item);

                return Commit(items);
            }
        }

        protected bool Update(T oldItem, T newItem)
        {
            lock (_locker)
            {
                var items = GetItems()
                    .ToList();

                if (!IsExistWithId(oldItem, items))
                {
                    return false;
                }

                var itemToUpdate = items
                    .First(j => j.Id == newItem.Id);

                if (IsExist(newItem, items))
                {
                    return false;
                }

                items[items.IndexOf(itemToUpdate)] = newItem;

                return Commit(items);
            }
        }

        protected bool Delete(T item)
        {
            lock (_locker)
            {
                var items = GetItems();

                if (!IsExistWithId(item, items))
                {
                    return false;
                }

                var itemToDelete = items
                    .First(i => i.Id == item.Id);

                var result = items.Remove(itemToDelete);

                return result && Commit(items);
            }
        }

        private bool Commit(IEnumerable<T> items)
        {
            try
            {
                var json = JsonSerializer.Serialize(items);

                File.WriteAllText(_filePath, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string ReadFromFile(string path)
        {
            string json = "[]";

            try
            {
                json = File.ReadAllText(path);
            }
            catch (FileNotFoundException)
            {
                File.WriteAllText(path, json);
            }

            return json;
        }

        private ICollection<T> GetItems()
        {
            var json = ReadFromFile(_filePath);

            try
            {
                var result = JsonSerializer.Deserialize<List<T>>(json);
                return result;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return null;
        }

        private int SetNewId(IEnumerable<T> items)
        {
            const int idIncrementStep = 1;

            var maxKey = items.Max(item => item.Id);

            var newKey = maxKey + idIncrementStep;

            return newKey;
        }

        protected abstract bool IsExist(T item, IEnumerable<T> items);

        protected abstract bool IsExistWithId(T item, IEnumerable<T> items);
    }
}