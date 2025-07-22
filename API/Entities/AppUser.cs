using System;

namespace API.Entities;

public class AppUser
{
    //kada dodje do entity frameworka ova klasa ce biti tabela, a polja ce biti kolone
    public int Id { get; set; } //access modifier ostaje public jer Entity Framework moze da radi samo sa public poljima
    //entity framework prepoznaje id i automatski zna da je to primarni kljuc, takodje automatski vodi racuna o njegovom inkrementiranju, tako da se ne ponavlja vrednost
    public required string UserName { get; set; } //pascal casing koristimo
    public required byte[] PasswordHash { get; set; } //for passsword hash
    public required byte[] PasswordSalt { get; set; } //password salt 
    

}
