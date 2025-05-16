using System;
using SlotMachineApp.Model;
using SlotMachineApp.Services;
using SlotMachineApp.Utilities;

var authService = new AuthService();
var slotService = new SlotMachineService();

bool loggedIn = false;
GameState state = GameState.Idle;
int putaran = 1;
int currentUserId = -1;
string currentUserRole = string.Empty;
string currentUsername = string.Empty;

while (true)
{
    Console.Clear();
    Console.WriteLine("=== SLOT MACHINE GAME ===");
    Console.WriteLine($"Status: {state}");

    if (loggedIn)
    {
        var balance = authService.GetUsers().FirstOrDefault(u => u.Username == currentUsername).Balance;
        Console.WriteLine($"Saldo Koin: {balance}");
    }
    else
    {
        Console.WriteLine($"Saldo koin: -");
    }
    Console.WriteLine();
    Console.WriteLine("1. Login");
    Console.WriteLine("2. Register");
    Console.WriteLine("3. Keluar");
    Console.WriteLine();
    Console.Write("Pilih Menu: ");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            Login(authService, ref loggedIn, ref currentUserId, ref currentUsername, ref currentUserRole);
            break;

        case "2":
            Register(authService, ref loggedIn, ref currentUserId, ref currentUsername, ref currentUserRole);
            break;

        case "3":
            state = GameState.Idle;
            return;

        default:
            Console.WriteLine("Pilihan Tidak Valid.");
            break;
    }

    if (loggedIn)
    {
        MainMenu(ref loggedIn, ref state, authService, slotService, currentUserRole);
    }
}

void Login(AuthService authService, ref bool loggedIn, ref int currentUserId, ref string currentUsername, ref string currentUserRole)
{
    
    Console.Clear();
    Console.WriteLine("=== SLOT MACHINE GAME ===");
    Console.WriteLine($"Status: {state}");
    Console.WriteLine($"Saldo Koin: -");
    Console.WriteLine();

    Console.Write("Username: ");
    var username = Console.ReadLine();
    Console.Write("Password: ");
    var password = Console.ReadLine();
    Console.WriteLine();
    try
    {
        if (authService.Login(username, password, out currentUserId, out currentUserRole, out int saldo))
        {
            loggedIn = true;
            currentUsername = username;
            state = GameState.LoggedIn;
            Console.Write($"Login Berhasil! Role: {currentUserRole}");
            Console.ReadLine();
        }
        else
        {
            Console.Write("Login gagal! Username Atau Password Salah.");
            Console.ReadLine();
        }
    }
    catch (Exception ex)
    {
        Console.Write($"Error: {ex.Message}");
        Console.ReadLine();
    }
}

void Register(AuthService authService, ref bool loggedIn, ref int currentUserId, ref string currentUsername, ref string currentUserRole)
{
    Console.Clear();
    Console.WriteLine("=== SLOT MACHINE GAME ===");
    Console.WriteLine($"Status: {state}");
    Console.WriteLine($"Saldo Koin: -");
    Console.WriteLine();
    Console.Write("Username Baru: ");
    var newUsername = Console.ReadLine();
    Console.Write("Password Baru: ");
    var newPassword = Console.ReadLine();
    Console.WriteLine();
    try
    {
        if (authService.Register(newUsername, newPassword, out currentUserId, out currentUserRole))
        {
            loggedIn = true;
            currentUsername = newUsername;
            state = GameState.LoggedIn;
            Console.Write("Registrasi Berhasil! Silakan Masuk.");
            Console.ReadLine();
        }
        else
        {
            Console.Write("Registrasi Gagal! Username Sudah Digunakan.");
            Console.ReadLine();
        }
    }
    catch (Exception ex)
    {
        Console.Write($"Error: {ex.Message}");
        Console.ReadLine();
    }
}

void MainMenu(ref bool loggedIn, ref GameState state, AuthService authService, SlotMachineService slotService, string currentUserRole)
{
    while (loggedIn && currentUserRole == "admin")
    {
        Console.Clear();
        Console.WriteLine("===== ADMIN MENU =====");

        var userBalance = authService.GetUsers().FirstOrDefault(u => u.Username == currentUsername).Balance;
        Console.WriteLine($"Penghasilan Anda: {userBalance}");
        Console.Write("\n");
        if (currentUserRole == "admin")
        {
            Console.WriteLine("1. Admin Dashboard");
            Console.WriteLine("2. Keluar");
            Console.Write("\n");
            Console.Write("Pilih menu: ");
            var gameInput = Console.ReadLine();

            switch (gameInput)
            {
                case "1":
                    if (currentUserRole == "admin")
                        AdminDashboard(authService);
                    else
                        loggedIn = false;
                    break;

                case "2":
                    state = GameState.Idle;
                    loggedIn = false;
                    break;

                default:
                    Console.WriteLine("Pilihan tidak valid.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("1. Main Slot (-100 koin)");
            Console.WriteLine("2. Keluar");

            Console.Write("Pilih Menu: ");
            var gameInput = Console.ReadLine();

            switch (gameInput)
            {
                case "1":
                    PlaySlot(authService, ref state, slotService);
                    break;

                case "2":
                    state = GameState.Idle;
                    loggedIn = false;
                    break;

                default:
                    Console.WriteLine("Pilihan Tidak Valid.");
                    break;
            }
        }
    }

    while (loggedIn && currentUserRole == "user")
    {
        Console.Clear();
        Console.WriteLine("=== SLOT MACHINE GAME ===");

        var userBalance = authService.GetUsers().FirstOrDefault(u => u.Username == currentUsername).Balance;
        Console.WriteLine($"Status: {state}");
        Console.WriteLine($"Koin Anda: {userBalance}");
        Console.Write("\n");
        if (currentUserRole == "admin")
        {
            Console.WriteLine("1. Admin Dashboard");
            Console.WriteLine("2. Keluar");
            Console.Write("\n");
            Console.Write("Pilih Menu: ");
            var gameInput = Console.ReadLine();

            switch (gameInput)
            {
                case "1":
                    if (currentUserRole == "admin")
                        AdminDashboard(authService);
                    else
                        loggedIn = false;
                    break;

                case "2":
                    state = GameState.Idle;
                    loggedIn = false;
                    break;

                default:
                    Console.WriteLine("Pilihan tidak valid.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("1. Main Slot (-100 koin)");
            Console.WriteLine("2. Casino Slot (-50 koin)");
            Console.WriteLine("3. Keluar");
            Console.Write("\n");
            Console.Write("Pilih Menu: ");
            var gameInput = Console.ReadLine();

            switch (gameInput)
            {
                case "1":
                    if (userBalance < 100)
                    {
                        Console.Write("\nSaldo Tidak Cukup Untuk Bermain.\nMungkin Permainan Selanjutnya Keberuntungan Di Pihak Anda :)");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {

                        putaran = 0;
                        authService.DecreaseBalance(currentUsername, 100);
                        state = GameState.Playing;
                        PlaySlot(authService, ref state, slotService);
                    }

                    break;

                case "2":
                    if (userBalance < 50)
                    {
                        Console.Write("\nSaldo Tidak Cukup Untuk Bermain.\nMungkin Permainan Selanjutnya Keberuntungan Di Pihak Anda :)");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        authService.DecreaseBalance(currentUsername, 50);
                        var (symbol1, symbol2, symbol3) = SlotMachine2.Spin();
                        Console.WriteLine($"|{symbol1} | {symbol2} | {symbol3}|");

                        WinType result = SlotMachine2.DetermineWin(symbol1, symbol2, symbol3);

                        Console.WriteLine($"{result}");

                        if (result == WinType.Jackpot)
                        {
                            authService.AddBalance(currentUsername, 500);
                            Console.WriteLine("Selamat! Anda mendapatkan Jackpot!");
                        }
                        else if (result == WinType.Lose)
                        {
                            authService.DecreaseBalance(currentUsername, 500);
                            Console.WriteLine("Sayang sekali! Anda kalah.");
                        }
                        else if (result == WinType.Wild)
                        {
                            authService.AddBalance(currentUsername, 150);
                            Console.WriteLine("Anda mendapatkan Wild! Lanjutkan bermain!");
                        }
                        else if (result == WinType.Scatter)
                        {
                            Console.WriteLine("Bonus! Dapatkan lebih banyak peluang!");
                            for (int i = 0; i < 10; i++)
                            {
                                (symbol1, symbol2, symbol3) = SlotMachine2.Spin();
                                Console.WriteLine($"|{symbol1} | {symbol2} | {symbol3}|");

                                result = SlotMachine2.DetermineWin(symbol1, symbol2, symbol3);

                                Console.WriteLine($"{result}");
                            }
                            
                            Console.WriteLine("Bonus! Dapatkan lebih banyak peluang!");
                        }
                        else if (result == WinType.FreeSpins)
                        {
                            (symbol1, symbol2, symbol3) = SlotMachine2.Spin();
                            Console.WriteLine($"|{symbol1} | {symbol2} | {symbol3}|");

                            result = SlotMachine2.DetermineWin(symbol1, symbol2, symbol3);

                            Console.WriteLine($"{result}");
                        }
                        else if (result == WinType.MegaWin)
                        {
                            authService.AddBalance(currentUsername, 50);
                            Console.WriteLine("Mega Win! Hadiah besar menanti!");
                        }
                        else if (result == WinType.SuperWin)
                        {
                            authService.AddBalance(currentUsername, 50);
                            Console.WriteLine("Super Win! Anda menang besar!");
                        }
                        else if (result == WinType.StickyWild)
                        {
                            authService.AddBalance(currentUsername, 50);
                            Console.WriteLine("Sticky Wild! Keuntungan lebih banyak!");
                        }
                        else
                        {
                            Console.WriteLine("Tidak ada kemenangan.");
                        }

                        Console.ReadLine() ;
                    }
                    break;

                case "3":
                    state = GameState.Idle;
                    loggedIn = false;
                    break;

                default:
                    Console.WriteLine("Pilihan Tidak Valid.");
                    break;
            }
        }
    }
}

void PlaySlot(AuthService authService, ref GameState state, SlotMachineService slotService)
{
    var userBalance = authService.GetUsers().FirstOrDefault(u => u.Username == currentUsername).Balance;
    Console.Clear();
    Console.WriteLine("=== SLOT MACHINE GAME ===");

    Console.WriteLine($"Status: {state}");
    Console.WriteLine($"Koin Anda: {userBalance}");





    if (userBalance > 0)
    {
        state = GameState.Playing;
    }else if (userBalance < 0)
    {
        state = GameState.GameOver;
    }
    

    Console.WriteLine($"\n===== PUTARAN {putaran++} =====");
    var rows = slotService.GenerateRows(rowCount: 5);
    rows.ForEach(Console.WriteLine);

    bool isJackpot = false;
    bool BombActive = false;

    foreach (var row in rows)
    {
        var items = row.Split(" | ");
        if (TableDrivenChecker.IsJackpot(items.ToList()))
        {
            if (items[0].ToLower() == "bomb  ")
            {
                putaran = 0;
                BombActive = true;
                authService.DecreaseBalance(currentUsername, userBalance);
                ApiLogger.LogResult("Bomb", userBalance);
                Console.WriteLine("\n💣 BOMB! GAME OVER 💣");

                state = GameState.GameOver;
                break;
            }
            else
            {
                isJackpot = true;
                
            }
        }
    }

    if (!BombActive) 
    {
        if (userBalance <= 0)
        {
            Console.WriteLine("\nSaldo Habis. Keluar Dari Permainan.");
        }
        else if (isJackpot)
        {
            authService.AddBalance(currentUsername, 1000);
            Console.WriteLine("\n🎉 JACKPOT! +1000 koin 🎉");
            ApiLogger.LogResult("jackpot", userBalance);
        }
        else
        {
            Console.WriteLine("\nBelum beruntung.");
            ApiLogger.LogResult("gagal", userBalance);
        }
    }

    if (BombActive)
    {
        Console.WriteLine("1. Kembali");
        Console.Write("\n");
        Console.Write("Pilih Menu: ");
        var gameInput = Console.ReadLine();

        switch (gameInput)
        {
            case "1":

                    MainMenu(ref loggedIn, ref state, authService, slotService, currentUserRole);
                    state = GameState.LoggedIn;
                break;

            default:
                Console.WriteLine("Pilihan Tidak Valid.");
                break;
        }
    }
    else
    {
        Console.WriteLine("1. Main Lagi (-100 koin)");
        Console.WriteLine("2. Kembali");
        Console.Write("\n");
        Console.Write("Pilih Menu: ");
        var gameInput = Console.ReadLine();

        switch (gameInput)
        {
            case "1":
                if (userBalance < 100)
                {
                    Console.Write("\nSaldo Tidak Cukup Untuk Bermain.\nMungkin Permainan Selanjutnya Keberuntungan Di Pihak Anda :)");
                    Console.ReadLine();
                    MainMenu(ref loggedIn, ref state, authService, slotService, currentUserRole);
                }
                else
                {
                    authService.DecreaseBalance(currentUsername, 100);
                    PlaySlot(authService, ref state, slotService);
                }
                break;

            case "2":
                state = GameState.LoggedIn;
                MainMenu(ref loggedIn, ref state, authService, slotService, currentUserRole);
                break;

            default:
                Console.WriteLine("Pilihan Tidak Valid.");
                break;
        }
    }
}

void AdminDashboard(AuthService authService)
{
    Console.Clear();
    Console.WriteLine("==== ADMIN DASHBOARD ====");
    Console.Write("\n");
    Console.WriteLine("1. View Users");
    Console.WriteLine("2. Tambah Saldo");
    Console.WriteLine("3. Exit to Main Game");
    Console.Write("\n");
    Console.Write("Pilih Menu: ");

    var adminInput = Console.ReadLine();
    Console.Write("\n");
    int index = 1;
    switch (adminInput)
    {
        case "1":
            Console.Clear();
            Console.WriteLine("===== USER LIST =====");
            Console.Write("\n");

            foreach (var user in authService.GetUsers())
            {

                if (user.Role == "admin")
                {
                    Console.WriteLine($"{index}. Username: {user.Username}\n   Password:{user.Password}\n   Role: {user.Role}");
                }
                else
                {
                    Console.WriteLine($"{index}. Username: {user.Username}\n   Password: {user.Password}\n   Role: {user.Role}\n   Balance: {user.Balance}");
                }
                index++;
                Console.Write("\n");
            }
            Console.Write("Tekan [ENTER] Untuk Kembali...");
            Console.ReadLine();
            break;

        case "2":
            tambahSaldo(authService);
            break;

        case "3":
            break;

        default:
            Console.Write("Invalid Choice.");
            break;
    }
}

void tambahSaldo(AuthService authService)
{
    int index = 1;
    Console.Clear();
    Console.WriteLine("==== TAMBAH SALDO ====");
    Console.Write("\n");

    foreach (var user in authService.GetUsers())
    {
        if (user.Role == "admin")
        {
            // Gak Ngapa Ngapain
        }
        else
        {
            Console.WriteLine($"{index}. Username: {user.Username}\n   Balance: {user.Balance}");
            index++;
            Console.Write("\n");
        }
        
    }

    Console.Write("Masukkan Nama Pengguna : ");
    var namauser = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(namauser))
    {
        Console.Write("\x1b[A");
        Console.Write("\x1b[2K");
        Console.WriteLine("Nama Pengguna Tidak Boleh Kosong!");
        Console.ReadLine();
        AdminDashboard(authService);
        return;
    }

    bool userFound = false;

    foreach (var user in authService.GetUsers())
    {
        if (user.Username == namauser)
        {
            userFound = true;
            bool isValidInput = false;
            int saldotambahan = 0;

            while (!isValidInput)
            {
                Console.Write("Masukkan Jumlah: ");
                var input = Console.ReadLine();

                isValidInput = int.TryParse(input, out saldotambahan);

                if (!isValidInput)
                {
                    Console.Write("\x1b[A");
                    Console.Write("\x1b[2K");
                }
                else if (saldotambahan < 0)
                {
                    Console.Write("\x1b[A");
                    Console.Write("\x1b[2K");
                    Console.Write("Nambah Anjir Bukan Ngurangin !!!");
                    break;
                }
                else
                {
                    authService.AddBalance(namauser, saldotambahan);
                    Console.Write($"Saldo Berhasil Ditambahkan Sebesar {saldotambahan} Koin");
                    break;
                }
            }
            break;
        }
    }

    if (!userFound)
    {
        Console.Write("\x1b[A");
        Console.Write("\x1b[2K");
        Console.Write("Nama Pengguna Tidak Ditemukan !!");
    }

    Console.ReadLine();
    AdminDashboard(authService);
}

