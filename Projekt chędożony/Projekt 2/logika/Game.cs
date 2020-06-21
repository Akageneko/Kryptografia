using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_2.logika
{
    class Game
    {
        int ile_graczy;
        string sciezka_cert;

        X509Certificate2 cert;
        string haslo_cert = "Qwerty";

        List<Uczestnik> uczestnicy = new List<Uczestnik>();
        List<bool> rsaUser = new List<bool>();
        List<Krypto> krypto = new List<Krypto>();
        bool rsaUsing = true;


        public Game(int ile_graczy, string certyfikaty)
        {
            this.ile_graczy = ile_graczy;
            this.sciezka_cert = certyfikaty;
            this.cert = new X509Certificate2(sciezka_cert + "servcert.p12", haslo_cert);


            for (int i = 1; i <= ile_graczy; i++) {
                krypto.Add(new Krypto(cert));

                uczestnicy.Add(new Uczestnik(i, sciezka_cert, this));
                rsaUser.Add(true);

            }
            MakeServer();

            foreach (Uczestnik u in uczestnicy) u.Greatings();
        }

        private void MakeServer()
        {

            

        }


        private void SEND(string s, int id)
        {
            byte[] wiadomosc;
            if (rsaUser[id-1])
            {
                wiadomosc = krypto[id-1].ZakodujRSA(s);
            }
            else
            {
                wiadomosc = krypto[id - 1].ZakodujAES(s);
            }


            uczestnicy[id-1].RECIVE(wiadomosc);
        }

        public void RECIVE(byte[] s, int id)
        {
            if (Encoding.UTF8.GetString(s).CompareTo("HelloClient") == 0)
            {
                ManageMessages(Encoding.UTF8.GetString(s), id);
                return;
            }

            string wiado;
            if (rsaUsing)
            {
                wiado = krypto[id - 1].OdkodujRSA(s);
            }
            else
            {
                wiado = krypto[id - 1].OdkodujAES(s);
            }
            ManageMessages(wiado,id);
        }

        private void ManageMessages(string s, int id)
        {
            string[] temp = s.Split('|');
            switch (temp[0])
            {
                case "HelloClient":
                        string ss = "HelloServer|" + krypto[id-1].GetPublicKeyXML(); 
                        SEND(ss,id);
                        
                    break;
                case "AESData":
                    krypto[id - 1].AESKeySet((temp[1]));
                    rsaUser[id - 1] = false;
                        
                    break;
                
            }
        }

        private void Print(String s)
        {
            Console.WriteLine("SERVER: {0}", s);
        }
    }
}
