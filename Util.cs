using SkiaSharp;

namespace Peter.BadgeMaker
{
    class Util
    {
        public static void PrintEmployees(List<Employee> employees)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                string template = "{0,-10}\t{1,-20}\t{2}";
                Console.WriteLine(
                    string.Format(
                        template,
                        employees[i].GetId(),
                        employees[i].GetFullName(),
                        employees[i].GetPhotoUrl()
                    )
                );
            }
        }

        public static void MakeCSV(List<Employee> employees)
        {
            if (!Directory.Exists("data"))
            {
                // create data direction if it doesn't exist
                Directory.CreateDirectory("data");
            }

            using (StreamWriter file = new("data/employees.csv")) // IDisposable
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    file.WriteLine(
                        "{0},{1},{2}",
                        employees[i].GetId(),
                        employees[i].GetFullName(),
                        employees[i].GetPhotoUrl()
                    );
                }
            }
        }

        async public static Task MakeBadges(List<Employee> employees)
        {
            const int BADGE_WIDTH = 669;
            const int BADGE_HEIGHT = 1044;
            const int PHOTO_LEFT_X = 184;
            const int PHOTO_TOP_Y = 215;
            const int PHOTO_RIGHT_X = 486;
            const int PHOTO_BOTTOM_Y = 517;
            const int COMPANY_NAME_Y = 150;
            const int EMPLOYEE_NAME_Y = 600;
            const int EMPLOYEE_ID_Y = 730;

            using(HttpClient client = new HttpClient())
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    Employee employee = employees[i];
                    // get photo from photoUrl
                    SKImage photo = SKImage.FromEncodedData(await client.GetStreamAsync(employee.GetPhotoUrl()));
                    // get badge background img, then create bitmap and canvas so we can draw on it
                    SKImage background = SKImage.FromEncodedData(File.OpenRead("badge.png"));
                    SKBitmap badge = new(BADGE_WIDTH, BADGE_HEIGHT);
                    SKCanvas canvas = new(badge);
                    // draw photo onto canvas
                    canvas.DrawImage(background, new SKRect(0, 0, BADGE_WIDTH, BADGE_HEIGHT));
                    canvas.DrawImage(photo, new SKRect(PHOTO_LEFT_X, PHOTO_TOP_Y, PHOTO_RIGHT_X, PHOTO_BOTTOM_Y ));
                    // configure text to be drawn onto canvas
                    SKPaint textPaint = new();
                    textPaint.TextSize = 42.0f;
                    textPaint.IsAntialias = true;
                    textPaint.Color = SKColors.White;
                    textPaint.IsStroke = false;
                    textPaint.TextAlign = SKTextAlign.Center;
                    textPaint.Typeface = SKTypeface.FromFamilyName("Arial");
                    // draw employee info onto canvas
                    canvas.DrawText(employee.GetCompanyName(), BADGE_WIDTH / 2, COMPANY_NAME_Y, textPaint);
                    textPaint.Color = SKColors.Black;
                    canvas.DrawText(employee.GetFullName(), BADGE_WIDTH / 2, EMPLOYEE_NAME_Y, textPaint);
                    textPaint.Typeface = SKTypeface.FromFamilyName("Courier New");
                    canvas.DrawText(employee.GetId().ToString(), BADGE_WIDTH / 2, EMPLOYEE_ID_Y, textPaint);

                    SKImage finalImage = SKImage.FromBitmap(badge);
                    SKData data = finalImage.Encode();
                    string filenameTemplate = "data/{0}_badge.png";
                    data.SaveTo(File.OpenWrite(string.Format(filenameTemplate, employee.GetId())));
                }
            }
        }
    }
}
