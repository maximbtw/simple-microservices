export enum AuthErrors {
    None = 0,

    // Login
    InvalidPasswordOrLogin,

    //Register
    UserWithSameEmailExists,
    UserWithSameLoginExists
}