using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodow.Models;

namespace System_Uznawania_Przychodow.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aktualizacja> Aktualizacjas { get; set; }

    public virtual DbSet<Firma> Firmas { get; set; }

    public virtual DbSet<Kategorium> Kategoria { get; set; }

    public virtual DbSet<Klient> Klients { get; set; }

    public virtual DbSet<Oplatum> Oplata { get; set; }

    public virtual DbSet<Oprogramowanie> Oprogramowanies { get; set; }

    public virtual DbSet<OprogramowanieAktualizacja> OprogramowanieAktualizacjas { get; set; }

    public virtual DbSet<OsobaFizyczna> OsobaFizycznas { get; set; }

    public virtual DbSet<Ratum> Rata { get; set; }

    public virtual DbSet<Rola> Rolas { get; set; }

    public virtual DbSet<Umowa> Umowas { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Znizka> Znizkas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

        }
    }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aktualizacja>(entity =>
        {
            entity.HasKey(e => e.IdAktualizacja).HasName("Aktualizacja_pk");

            entity.ToTable("Aktualizacja");

            entity.Property(e => e.IdAktualizacja).ValueGeneratedNever();
            entity.Property(e => e.Opis).HasMaxLength(250);
        });

        modelBuilder.Entity<Firma>(entity =>
        {
            entity.HasKey(e => e.IdKlient).HasName("Firma_pk");

            entity.ToTable("Firma");

            entity.HasIndex(e => e.IdKlient, "Firma_ak_1").IsUnique();

            entity.HasIndex(e => e.Krs, "Firma_idx_1");

            entity.Property(e => e.IdKlient).ValueGeneratedNever();
            entity.Property(e => e.Krs)
                .HasMaxLength(10)
                .HasColumnName("KRS");
            entity.Property(e => e.Nazwa).HasMaxLength(100);

            entity.HasOne(d => d.IdKlientNavigation).WithOne(p => p.Firma)
                .HasForeignKey<Firma>(d => d.IdKlient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Firma_Klient");
        });

        modelBuilder.Entity<Kategorium>(entity =>
        {
            entity.HasKey(e => e.IdKategoria).HasName("Kategoria_pk");

            entity.Property(e => e.IdKategoria).ValueGeneratedNever();
            entity.Property(e => e.Nazwa).HasMaxLength(100);
        });

        modelBuilder.Entity<Klient>(entity =>
        {
            entity.HasKey(e => e.IdKlient).HasName("Klient_pk");

            entity.ToTable("Klient");

            entity.Property(e => e.IdKlient).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.NrTelefonu).HasMaxLength(20);
        });

        modelBuilder.Entity<Oplatum>(entity =>
        {
            entity.HasKey(e => e.IdOplata).HasName("Oplata_pk");

            entity.Property(e => e.IdOplata).ValueGeneratedNever();
            entity.Property(e => e.CzyZwrocone).HasColumnName("czy_zwrocone");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Wartosc).HasColumnType("money");

            entity.HasOne(d => d.IdKlientNavigation).WithMany(p => p.Oplata)
                .HasForeignKey(d => d.IdKlient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Oplata_Klient");

            entity.HasOne(d => d.IdUmowaNavigation).WithMany(p => p.Oplata)
                .HasForeignKey(d => d.IdUmowa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Oplata_Umowa");
        });

        modelBuilder.Entity<Oprogramowanie>(entity =>
        {
            entity.HasKey(e => e.IdOprogramowanie).HasName("Oprogramowanie_pk");

            entity.ToTable("Oprogramowanie");

            entity.Property(e => e.IdOprogramowanie).ValueGeneratedNever();
            entity.Property(e => e.Nazwa).HasMaxLength(100);
            entity.Property(e => e.Opis).HasMaxLength(250);
            entity.Property(e => e.Wersja).HasMaxLength(100);

            entity.HasOne(d => d.IdKategoriaNavigation).WithMany(p => p.Oprogramowanies)
                .HasForeignKey(d => d.IdKategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Oprogramowanie_Kategoria");
        });

        modelBuilder.Entity<OprogramowanieAktualizacja>(entity =>
        {
            entity.HasKey(e => new { e.IdOprogramowanie, e.IdAktualizacja }).HasName("Oprogramowanie_Aktualizacja_pk");

            entity.ToTable("Oprogramowanie_Aktualizacja");

            entity.HasOne(d => d.IdAktualizacjaNavigation).WithMany(p => p.OprogramowanieAktualizacjas)
                .HasForeignKey(d => d.IdAktualizacja)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Oprogramowanie_Aktualizacja_Aktualizacja");

            entity.HasOne(d => d.IdOprogramowanieNavigation).WithMany(p => p.OprogramowanieAktualizacjas)
                .HasForeignKey(d => d.IdOprogramowanie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Oprogramowanie_Aktualizacja_Oprogramowanie");
        });

        modelBuilder.Entity<OsobaFizyczna>(entity =>
        {
            entity.HasKey(e => e.IdKlient).HasName("Osoba_Fizyczna_pk");

            entity.ToTable("Osoba_Fizyczna");

            entity.HasIndex(e => e.Pesel, "Osoba_Fizyczna_ak_1").IsUnique();

            entity.HasIndex(e => e.Pesel, "Osoba_Fizyczna_idx_2");

            entity.Property(e => e.IdKlient).ValueGeneratedNever();
            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.IsDeleted).HasColumnName("Is_deleted");
            entity.Property(e => e.Nazwisko).HasMaxLength(50);
            entity.Property(e => e.Pesel)
                .HasMaxLength(11)
                .HasColumnName("PESEL");

            entity.HasOne(d => d.IdKlientNavigation).WithOne(p => p.OsobaFizyczna)
                .HasForeignKey<OsobaFizyczna>(d => d.IdKlient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Osoba_Fizyczna_Klient");
        });

        modelBuilder.Entity<Ratum>(entity =>
        {
            entity.HasKey(e => e.IdRata).HasName("Rata_pk");

            entity.Property(e => e.IdRata).ValueGeneratedNever();
        });

        modelBuilder.Entity<Rola>(entity =>
        {
            entity.HasKey(e => e.IdRola).HasName("Rola_pk");

            entity.ToTable("Rola");

            entity.Property(e => e.IdRola).ValueGeneratedNever();
            entity.Property(e => e.Nazwa).HasMaxLength(50);
        });

        modelBuilder.Entity<Umowa>(entity =>
        {
            entity.HasKey(e => e.IdUmowa).HasName("Umowa_pk");

            entity.ToTable("Umowa");

            entity.Property(e => e.IdUmowa).ValueGeneratedNever();
            entity.Property(e => e.Cena).HasColumnType("money");

            entity.HasOne(d => d.IdOprogramowanieNavigation).WithMany(p => p.Umowas)
                .HasForeignKey(d => d.IdOprogramowanie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Umowa_Oprogramowanie");

            entity.HasOne(d => d.IdRataNavigation).WithMany(p => p.Umowas)
                .HasForeignKey(d => d.IdRata)
                .HasConstraintName("Umowa_Rata");

            entity.HasOne(d => d.IdZnizkaNavigation).WithMany(p => p.Umowas)
                .HasForeignKey(d => d.IdZnizka)
                .HasConstraintName("Umowa_Znizka");

            entity.HasOne(d => d.OdbiorcaNavigation).WithMany(p => p.UmowaOdbiorcaNavigations)
                .HasForeignKey(d => d.Odbiorca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Umowa_Klient_Odbiorca");

            entity.HasOne(d => d.SprzedawcaNavigation).WithMany(p => p.UmowaSprzedawcaNavigations)
                .HasForeignKey(d => d.Sprzedawca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Umowa_Klient_Sprzedawca");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("User_pk");

            entity.ToTable("User");

            entity.Property(e => e.IdUser).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Salt).HasMaxLength(50);

            entity.HasOne(d => d.IdRolaNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRola)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Rola");
        });

        modelBuilder.Entity<Znizka>(entity =>
        {
            entity.HasKey(e => e.IdZnizka).HasName("Znizka_pk");

            entity.ToTable("Znizka");

            entity.Property(e => e.IdZnizka).ValueGeneratedNever();
            entity.Property(e => e.Nazwa).HasMaxLength(100);
            entity.Property(e => e.OkresDo).HasColumnType("datetime");
            entity.Property(e => e.OkresOd).HasColumnType("datetime");
            entity.Property(e => e.Wartosc).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
