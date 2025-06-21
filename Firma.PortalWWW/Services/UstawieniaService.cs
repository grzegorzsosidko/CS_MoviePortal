using Firma.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Services
{
    public class UstawieniaService : IUstawieniaService
    {
        private readonly IDictionary<string, string> _ustawienia;

        // Wstrzykuję IServiceProvider, aby móc ręcznie utworzyć tymczasowy
        // zakres (scope) i bezpiecznie pobrać z niego usługę o krótszym cyklu życia (FirmaContext).
        public UstawieniaService(IServiceProvider serviceProvider)
        {
            // Tworzę tymczasowy zakres DI, aby uniknąć problemu "captive dependency",
            // czyli wstrzykiwania usługi 'Scoped' do 'Singletona'.
            using (var scope = serviceProvider.CreateScope())
            {
                // Wewnątrz tego zakresu pobieram instancję DbContext.
                // Ta instancja będzie żyła tylko w obrębie tego bloku 'using'.
                var context = scope.ServiceProvider.GetRequiredService<FirmaContext>();

                // Wczytuję wszystkie ustawienia z bazy danych do słownika w pamięci.
                // Robię to tylko raz, podczas tworzenia instancji tego serwisu (przy starcie aplikacji).
                _ustawienia = context.Ustawienie.ToDictionary(u => u.Klucz, u => u.Wartosc);
            }
            // Po wyjściu z bloku 'using', zakres i pobrany z niego DbContext są niszczone.
            // Serwis (jako Singleton) przechowuje już tylko wczytane dane w słowniku.
        }

        // Metoda zwraca wartość dla danego klucza ze słownika w pamięci.
        public string GetValue(string key)
        {
            return _ustawienia.TryGetValue(key, out var value) ? value : string.Empty;
        }
    }
}