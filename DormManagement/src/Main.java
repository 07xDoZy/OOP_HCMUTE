import dao.DormContext;

public class Main {
    public static void main(String[] args) {
        System.out.println("============================================");
        System.out.println("   DORM MANAGEMENT SYSTEM - 30 CAU TRUY VAN");
        System.out.println("============================================\n");

        System.out.println(">>>>>>>>>> THANH VIEN 1: KIEU <<<<<<<<<<");
        MainQuery.main(new String[0]);

        System.out.println("\n>>>>>>>>>> THANH VIEN 2 + 3: NHUNG, LOC <<<<<<<<<<");
        MainQuery2.main(new String[0]);
    }
}