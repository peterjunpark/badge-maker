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

            using(HttpClient client = new HttpClient())
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    string photoUrl = employees[i].GetPhotoUrl();
                    SKImage photo = SKImage.FromEncodedData(await client.GetStreamAsync(employees[i].GetPhotoUrl()));
                    SKImage background = SKImage.FromEncodedData(File.OpenRead("badge.png"));

                    SKBitmap badge = new SKBitmap(BADGE_WIDTH, BADGE_HEIGHT);
                    SKCanvas canvas = new SKCanvas(badge);

                    canvas.DrawImage(background, new SKRect(0, 0, BADGE_WIDTH, BADGE_HEIGHT));
                    canvas.DrawImage(photo, new SKRect(PHOTO_LEFT_X, PHOTO_TOP_Y, PHOTO_RIGHT_X, PHOTO_BOTTOM_Y ));

                    SKImage finalImage = SKImage.FromBitmap(badge);
                    SKData data = finalImage.Encode();
                    data.SaveTo(File.OpenWrite("data/employeeBadge.png"));
                }
            }
        }
    }
}
