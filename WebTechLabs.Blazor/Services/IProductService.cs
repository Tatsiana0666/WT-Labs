using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebTechLabs.Blazor.Services
{
    // Интерфейс сервиса продуктов
    public interface IProductService<T> where T : class
    {
        event Action ListChanged; // Событие изменения списка
        IEnumerable<T> Products { get; } // Список объектов
        int CurrentPage { get; } // Номер текущей страницы
        int TotalPages { get; } // Общее количество страниц
        Task GetProducts(int pageNo = 1, int pageSize = 3); // Получение списка объектов
    }

    // Реализация сервиса API для продуктов
    public class ApiProductService : IProductService<Dish>
    {
        private readonly HttpClient _httpClient; // HttpClient для работы с API
        private List<Dish> _dishes = new(); // Список блюд
        private int _currentPage = 1; // Текущая страница
        private int _totalPages = 1; // Общее количество страниц

        public IEnumerable<Dish> Products => _dishes; // Возвращает список блюд
        public int CurrentPage => _currentPage; // Возвращает текущую страницу
        public int TotalPages => _totalPages; // Возвращает общее количество страниц

        public event Action ListChanged; // Событие изменения списка

        // Конструктор с внедрением HttpClient
        public ApiProductService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        // Получение списка продуктов с пагинацией
        public async Task GetProducts(int pageNo = 1, int pageSize = 3)
        {
            // URL сервиса API
            var uri = _httpClient.BaseAddress?.AbsoluteUri ?? throw new InvalidOperationException("BaseAddress is not set.");

            // Данные для запроса
            var queryData = new Dictionary<string, string>
            {
                { "pageNo", pageNo.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            // Формирование строки запроса
            var queryString = string.Join("&", queryData.Select(kv => $"{kv.Key}={kv.Value}"));

            // Отправка HTTP-запроса
            var result = await _httpClient.GetAsync($"{uri}?{queryString}");

            if (result.IsSuccessStatusCode)
            {
                // Получение данных из ответа
                var responseData = await result.Content.ReadFromJsonAsync<ResponseData<ProductListModel<Dish>>>();

                if (responseData?.Data != null)
                {
                    // Обновление данных
                    _currentPage = responseData.Data.CurrentPage;
                    _totalPages = responseData.Data.TotalPages;
                    _dishes = responseData.Data.Items ?? new List<Dish>();
                    ListChanged?.Invoke(); // Уведомление об изменении списка
                }
            }
            else
            {
                // Обработка ошибки
                _dishes = new List<Dish>();
                _currentPage = 1;
                _totalPages = 1;
            }
        }
    }

    // Пример модели блюда
    public class Dish
    {
        public string Name { get; set; }
        public string Image { get; set; } // Ссылка на изображение
        public string Description { get; set; } // Описание блюда
        public int Calories { get; set; } // Количество калорий
    }

    // Обёртка для данных ответа API
    public class ResponseData<T>
    {
        public T Data { get; set; }
    }

    // Модель списка продуктов с пагинацией
    public class ProductListModel<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }
}