using Microsoft.AspNetCore.Mvc;

public class PhonesController : Controller
{
    private MobileContext _db;

    public PhonesController(MobileContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        List<Phone> phones = _db.Phones.ToList();
        return View(phones);
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Phone phone)
    {
        if (phone != null)
        {
            phone.DescriptionFile ??= string.Empty; 

            _db.Phones.Add(phone);
            _db.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    public IActionResult DownloadDescription(string company)
    {
        Console.WriteLine($"Downloading company: '{company}'");
        if (string.IsNullOrEmpty(company))
        {
            return NotFound();
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Descriptions", $"{company}.txt");

        Console.WriteLine(filePath);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        var fileName = $"{company}.txt";
        return File(fileBytes, "application/octet-stream", fileName);
    }



    public IActionResult RedirectToManufacturer(string company)
    {
        if (string.IsNullOrEmpty(company))
        {
            return NotFound();
        }

        Console.WriteLine($"Redirecting company: '{company}'");

        var manufacturerUrls = new Dictionary<string, string>
    {
        { "Apple", "https://www.apple.com" },
        { "Samsung", "https://www.samsung.com" },
        { "Xiaomi", "https://www.mi.com" },
        { "OnePlus", "https://www.oneplus.com" },
        { "Google", "https://store.google.com" }
    };

        if (manufacturerUrls.TryGetValue(company, out string url))
        {
            return Redirect(url);
        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult Edit(int id)
    {
        var phone = _db.Phones.FirstOrDefault(p => p.Id == id);
        if (phone == null)
        {
            return NotFound();
        }
        return View(phone); 
    }

    [HttpPost]
    public IActionResult Edit(Phone phone)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage); 
            }
            return View(phone);
        }

        if (ModelState.IsValid)
        {

            var existingPhone = _db.Phones.FirstOrDefault(p => p.Id == phone.Id);
            if (existingPhone == null)
            {
                return NotFound();
            }

            existingPhone.Name = phone.Name;
            existingPhone.Company = phone.Company;
            existingPhone.Price = phone.Price;

            _db.SaveChanges();
            return RedirectToAction("Index"); 
        }
        return View(phone); 
    }

    public IActionResult Delete(int id)
    {
        var phone = _db.Phones.FirstOrDefault(p => p.Id == id);

        if (phone == null)
        {
            return NotFound();
        }

        _db.Phones.Remove(phone);
        _db.SaveChanges(); 

        return RedirectToAction("Index");
    }


}