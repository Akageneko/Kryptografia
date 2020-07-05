using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Math.Gmp.Native;

namespace Projekt_2.logika
{
    class Uczestnik
    {
        private int ID;
        string sciezka_cert;
        X509Certificate2 cert;
        string haslo_cert = "Qwerty";

        bool pierwszy = true;
        Random rand = new Random();

        Krypto krypto;
        bool rsaUsing = true;

        Game game;


        mpz_t g;
        mpz_t p;

        mpz_t gy;
        mpz_t xi;




        public Uczestnik(int id, string sciezka_cert, Game game)
        {
            this.game = game;
            this.ID = id;
            this.sciezka_cert = sciezka_cert + "g"+ID.ToString()+"cert.p12";
            this.cert = new X509Certificate2(this.sciezka_cert, haslo_cert);
            krypto = new Krypto(cert);

            g = new mpz_t();
            p = new mpz_t();
            gy = new mpz_t();
            xi = new mpz_t();
            gmp_lib.mpz_init(g);
            gmp_lib.mpz_init(p);
            

        }

        public void Greatings()
        {
            string s = "HelloClient";
            //s = s + krypto.GetPublicKeyXML();
            //Print(s);
            SEND(s, true);
        }

        private void SendAESData()
        {
            string s = "AESData|"+ BitConverter.ToString( krypto.AESKeyGet()) ;
            SEND(s, true);
            rsaUsing = false;
            pierwszy = false;
           // Print("koniec powitania");
        }

        private void ActualGame()
        {
        }

        private void UstalGrupęIgx(string gs, string ps)
        {
            g = (mpz_t)gs;
            p = (mpz_t)ps;
            //int x = rand.Next(22)+1;
            int x = rand.Next() + 1;
            gmp_lib.mpz_init(xi);
            xi = (mpz_t)x.ToString();
            gmp_lib.mpz_powm(xi, g, xi, p);

            mpz_t gxi = new mpz_t();
            gmp_lib.mpz_init(gxi);
            
            gmp_lib.mpz_powm(gxi,g,xi,p); //g do xi mod p


            mpz_t v = new mpz_t();
            gmp_lib.mpz_init(v);
            x = rand.Next() + 1;
            v = (mpz_t)x.ToString();

            mpz_t gv = new mpz_t();
            gmp_lib.mpz_init(gv);
            gmp_lib.mpz_powm(gv, g, v, p);

            byte[] h;
            SHA1 sha = new SHA1CryptoServiceProvider();
            h = sha.ComputeHash(Encoding.ASCII.GetBytes(gv.ToString() + gxi.ToString()));
            string sh = BitConverter.ToString(h);
            sh = sh.Replace("-","");

            mpz_t hl = new mpz_t();
            gmp_lib.mpz_init(hl);
            mpz_t tmp = new mpz_t();
            gmp_lib.mpz_init(tmp);
            uint j = 0;
            
            for(int i= sh.Length - 1; i >= 0; i--)
            {
                tmp = (mpz_t)Convert.ToInt32(sh[i].ToString(),16).ToString();
                gmp_lib.mpz_pow_ui(tmp, tmp, j);
                gmp_lib.mpz_add(hl, hl, tmp);
                j++;
            }

            mpz_t r = new mpz_t();
            gmp_lib.mpz_init(r);
            gmp_lib.mpz_mul(r, xi, hl);
            gmp_lib.mpz_sub(r, v, r);    








            SEND("GXI|"+gxi.ToString()+"|"+gv.ToString()+"|"+r.ToString()+"|"+hl.ToString() , false);

            //DOŁOZYĆ ZERO KNOWLEGE PROOF
        }

        private void ObliczGY(string[] sgxi)
        {
            int i;
            gmp_lib.mpz_init_set_si(gy, 1);
            for (i = 1; i< sgxi.Count(); i += 2)
            {
                int xid = Int32.Parse(sgxi[i]);
                mpz_t gxi = new mpz_t();
                gmp_lib.mpz_init(gxi);
                gxi = (mpz_t)sgxi[i + 1];
                if (sgxi[i + 1].CompareTo("0") == 0)
                {
                    gmp_lib.mpz_init_set_si(gxi, 1);
                }
                
                if(xid < ID)
                {
                    gmp_lib.mpz_mul(gy, gy, gxi);
                    gmp_lib.mpz_mod(gy, gy, p);
                }

                if (xid > ID)
                {
                    gmp_lib.mpz_invert(gxi, gxi, p);
                    gmp_lib.mpz_mul(gy, gy, gxi);
                    gmp_lib.mpz_mod(gy, gy, p);
                }


            }

            //Print("gy: "+gy.ToString());
            gmp_lib.mpz_powm(gy, gy, xi, p);

            //Print("gyx: " + gy.ToString());
        }

        private void Glosuj()
        {
            Print("Głosujesz Y/N:");
            char glos = Console.ReadLine()[0];
            if(glos == 'Y' || glos == 'y')
            {
                Print("Zaglosowales na TAK");
                gmp_lib.mpz_mul(gy,gy,g);
                gmp_lib.mpz_mod(gy,gy,p);
            }
            else
            {
                Print("Zaglosowales na NIE");
            }
           

            SEND("GLOS|"+gy.ToString() ,false);
            //DOROBIĆ ZERO KNOWLEGE 1 of 2 na glos!!!!
        }

        private void Wynik(string[] sgxi)
        {
            mpz_t wynik = new mpz_t();
            gmp_lib.mpz_init_set_si(wynik, 1);
            for (int i = 1; i < sgxi.Count(); i += 2)
            {
                mpz_t gxi = new mpz_t();
                gmp_lib.mpz_init(gxi);
                gxi = (mpz_t)sgxi[i + 1];
                if (sgxi[i + 1].CompareTo("0") == 0)
                {
                    gmp_lib.mpz_init_set_si(gxi, 1);
                }

                gmp_lib.mpz_mul(wynik, wynik, gxi);
                gmp_lib.mpz_mod(wynik, wynik, p);

            }

            int j = 1;
            mpz_t wyn = new mpz_t();
            gmp_lib.mpz_init(wyn);
            wyn = (mpz_t)g.ToString();
            
            while (gmp_lib.mpz_cmp(wyn, wynik) != 0)
            {
                gmp_lib.mpz_mul(wyn, wyn, g);
                gmp_lib.mpz_mod(wyn, wyn, p);
                j++;
            }

            Print("wynik to: " + j.ToString());// +" "+ wynik.ToString()+" " + wyn.ToString());
            SEND("FINITE",false);
        }




        private void SEND(string s, bool rsa)
        {
            byte[] wiadomosc;
            if (rsa)
            {
                wiadomosc = krypto.ZakodujRSA(s);
            }
            else
            {
                wiadomosc = krypto.ZakodujAES(s);
            }


            game.RECIVE(wiadomosc, ID);
        }

        public void RECIVE(byte[] s)
        {
            string wiado;
            if (pierwszy)
            {
                wiado = Encoding.UTF8.GetString(s);
                
            }
            else
            {

                if (rsaUsing)
                {
                    wiado = krypto.OdkodujRSA(s);
                }
                else
                {
                    wiado = krypto.OdkodujAES(s);
                }
            }
            ManageMessages(wiado);
        }

        private void ManageMessages(string s)
        {
            string[] temp = s.Split('|');
            Print(s);
            switch (temp[0])
            {
                case "HelloServer":
                    krypto.Set_HisPubklicKey(temp[1]);
                    pierwszy = false;
                    SendAESData();
                    
                    break;
                
                case "OkAES":

                    break;

                case "GrupaDane":
                    UstalGrupęIgx(temp[1], temp[2]);
                    break;

                case "GotoweDoGY":
                    ObliczGY(temp);
                    break;

                case "Glosuj":
                    Glosuj();
                    break;

                case "Wyniki":
                    Wynik(temp);
                    break;

            }

        }

        private void Print(String s)
        {
            Console.WriteLine("Uczestnik {0}: {1}",ID,s);
        }

    }
}
