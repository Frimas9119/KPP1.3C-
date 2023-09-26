using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
class Product
{
    public string Name { get; set; }
    public string Unit { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime ArrivalDate { get; set; }
    public string Description { get; set; }

    public Product(string name, string unit, int quantity, decimal price, DateTime arrivalDate, string description)
    {
        Name = name;
        Unit = unit;
        Quantity = quantity;
        Price = price;
        ArrivalDate = arrivalDate;
        Description = description;
    }
}

[Serializable]
class ProductContainer : IEnumerable<Product>
{
    private List<Product> products = new List<Product>();

    public void AddProduct(Product product)
    {
        products.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        products.Remove(product);
    }

    public void UpdateProduct(Product oldProduct, Product newProduct)
    {
        int index = products.IndexOf(oldProduct);
        if (index >= 0)
        {
            products[index] = newProduct;
        }
    }

    public void Serialize(string fileName)
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
        }
    }

    public static ProductContainer Deserialize(string fileName)
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Open))
        {
            IFormatter formatter = new BinaryFormatter();
            return (ProductContainer)formatter.Deserialize(stream);
        }
    }

    public IEnumerator<Product> GetEnumerator()
    {
        return products.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Створення контейнера товарів
        ProductContainer container = new ProductContainer();

        // Додавання товарів до контейнера
        container.AddProduct(new Product("Товар1", "шт.", 10, 100, DateTime.Now, "Опис товару 1"));
        container.AddProduct(new Product("Товар2", "кг", 5, 50, DateTime.Now, "Опис товару 2"));

        // Серіалізація контейнера
        container.Serialize("products.dat");

        // Десеріалізація контейнера
        ProductContainer deserializedContainer = ProductContainer.Deserialize("products.dat");

        // Перевірка десеріалізованих товарів
        foreach (var product in deserializedContainer)
        {
            Console.WriteLine($"Найменування: {product.Name}, Кількість: {product.Quantity}, Ціна: {product.Price}");
        }
    }
}
