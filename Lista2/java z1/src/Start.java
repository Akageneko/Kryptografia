import java.io.*;
import java.util.*;


class Interf{


    Scanner in;
    EncDec encdec;
    public Interf(){
        in = new Scanner(System.in);
        encdec = new EncDec();
    }

    public void Start() throws FileNotFoundException {

        String tryb;
        String keystorePass;
        String keystoreAlias;
        String keystoreFile;
        String input;
        String output;

        String s;
        System.out.println("Czy chcesz użyć pliku konfigurcyjnego? Y/y - tak:");
        s = in.nextLine();
        System.out.println(s);
        if (s.toLowerCase().compareTo("y") == 0){
            System.out.println("Podaj ścieżkę do pliku konfiguracyjnego:");
            s = in.nextLine();
            File config_file = new File(s);
            Scanner config = new Scanner(config_file);

            tryb = config.nextLine();
            keystorePass = config.nextLine();
            keystoreAlias = config.nextLine();
            keystoreFile = config.nextLine();
            input = config.nextLine();
            output = config.nextLine();

        }else{
            System.out.println("Podaj tryb algorytmu:");
            tryb = in.nextLine();
            System.out.println("Podaj hasło do keystore:");
            keystorePass = in.nextLine();
            System.out.println("Podaj alias do klucza:");
            keystoreAlias = in.nextLine();
            System.out.println("Podaj ścieżkę do pliku keystore:");
            keystoreFile = in.nextLine();
            System.out.println("Podaj ścieżkę do pliku wejściowego:");
            input = in.nextLine();
            System.out.println("Podaj ścieżkę do pliku wyjściowego:");
            output = in.nextLine();
        }

        InputStream keystore = new FileInputStream(keystoreFile);
        File infile = new File(input);
        File outfile = new File(output);

        System.out.println("Czy chcesz zaszfrować podany plik? Y/y - tak (inny przycisk oznacza deszyfrację):");
        s = in.nextLine();
        if (s.toLowerCase().compareTo("y") == 0){
            encdec.encrypt(tryb,keystore,keystorePass,keystoreAlias,infile,outfile);
        }else{
            encdec.decrypt(tryb,keystore,keystorePass,keystoreAlias,infile,outfile);
        }


    }


    public void Oracle() throws FileNotFoundException {
        String tryb;
        String keystorePass;
        String keystoreAlias;
        String keystoreFile;
        ArrayList<String> input = new ArrayList<>();
        int numbInput;

        String s;
        System.out.println("Czy chcesz użyć pliku konfigurcyjnego? Y/y - tak:");
        s = in.nextLine();
        if (s.toLowerCase().compareTo("y") == 0){
            System.out.println("Podaj ścieżkę do pliku konfiguracyjnego:");
            s = in.nextLine();
            File config_file = new File(s);
            Scanner config = new Scanner(config_file);
            tryb = config.nextLine();
            keystorePass = config.nextLine();
            keystoreAlias = config.nextLine();
            keystoreFile = config.nextLine();

        }else{
            System.out.println("Podaj tryb algorytmu:");
            tryb = in.nextLine();
            System.out.println("Podaj hasło do keystore:");
            keystorePass = in.nextLine();
            System.out.println("Podaj alias do klucza:");
            keystoreAlias = in.nextLine();
            System.out.println("Podaj ścieżkę do pliku keystore:");
            keystoreFile = in.nextLine();

        }

        System.out.println("Ile będzie wiadomości:");
        numbInput = Integer.parseInt(in.nextLine());
        while(numbInput > 0){
            System.out.println("Podaj wiadomość");
            s = in.nextLine();
            input.add(s);
            numbInput--;
        }

        InputStream keystore = new FileInputStream(keystoreFile);
        System.out.println("zaszyfrowane wiadomości to:");
        for (String ss: input) {
            System.out.println(encdec.encrypt(tryb,keystore,keystorePass,keystoreAlias,ss));
        }

    }

    public void Challenge() throws FileNotFoundException {
        String tryb;
        String keystorePass;
        String keystoreAlias;
        String keystoreFile;
        ArrayList<String> input = new ArrayList<>();
        int numbInput;

        String s;
        System.out.println("Czy chcesz użyć pliku konfigurcyjnego? Y/y - tak:");
        s = in.nextLine();
        System.out.println(s);
        if (s.toLowerCase().compareTo("y") == 0){
            System.out.println("Podaj ścieżkę do pliku konfiguracyjnego:");
            s = in.nextLine();
            File config_file = new File(s);
            Scanner config = new Scanner(config_file);
            tryb = config.nextLine();
            keystorePass = config.nextLine();
            keystoreAlias = config.nextLine();
            keystoreFile = config.nextLine();

        }else{
            System.out.println("Podaj tryb algorytmu:");
            tryb = in.nextLine();
            System.out.println("Podaj hasło do keystore:");
            keystorePass = in.nextLine();
            System.out.println("Podaj alias do klucza:");
            keystoreAlias = in.nextLine();
            System.out.println("Podaj ścieżkę do pliku keystore:");
            keystoreFile = in.nextLine();

        }

        InputStream keystore = new FileInputStream(keystoreFile);

        System.out.println("Podaj wiadomość m0:");
        s = in.nextLine();
        input.add(s);

        System.out.println("Podaj wiadomość m1:");
        s = in.nextLine();
        input.add(s);

        System.out.println("zaszyfrowana wiadomość to:");
        Random rand = new Random();
        int b = rand.nextBoolean() ? 1 : 0;
        if(b == 0){
            System.out.println(encdec.encrypt(tryb,keystore,keystorePass,keystoreAlias,input.get(0)));
        }else{
            System.out.println(encdec.encrypt(tryb,keystore,keystorePass,keystoreAlias,input.get(1)));
        }

        System.out.println("Która wiadomość została zaszyfrowana? 0 albo 1");
        s = in.nextLine();
        if( Integer.parseInt(s) == b){
            System.out.println("Poprawnie");
        }else {
            System.out.println("Błąd");
        }

    }

}



public class Start {


    public static void main(String[] args)  {

       // EncDec encdec = new EncDec();
        Interf interd = new Interf();

       // String alg = "CBC";
       // String passKey = "Qwerty";
       // String aliasKey = "Matai";
        try {
           // InputStream keystore = new FileInputStream("C:\\Users\\Matai\\Desktop\\infa\\s8\\Krypto\\Lista2\\java z1\\out\\production\\AESEncDec\\keystore.keystore");
           // File infile = new File("C:\\Users\\Matai\\Desktop\\infa\\s8\\Krypto\\Lista2\\java z1\\out\\production\\AESEncDec\\in.txt");
           // File outfile = new File("C:\\Users\\Matai\\Desktop\\infa\\s8\\Krypto\\Lista2\\java z1\\out\\production\\AESEncDec\\out.txt");
            //encdec.encrypt(alg,keystore,passKey,aliasKey,infile,outfile);
           // encdec.decrypt(alg,keystore,passKey,aliasKey,outfile,infile);
           // keystore.close();

            System.out.println("Co chcesz zrobić:\n" +
                    "1  zaszyfruj/odszyfruj plik\n" +
                    "2  encryption oracle\n" +
                    "3  challenge");
            Scanner in = new Scanner(System.in);
            switch (in.nextLine()){
                case "1":
                    interd.Start();
                    break;
                case "2":
                    interd.Oracle();
                    break;
                case "3":
                    interd.Challenge();
                    break;
                default:
                    System.out.println("Wrong input. Terminate");

            }






        }
        catch (Exception e){
            System.out.println(e);
            System.out.println(e.getStackTrace());
        }

    }


}
