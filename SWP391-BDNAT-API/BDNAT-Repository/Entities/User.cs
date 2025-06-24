using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? PasswordHash { get; set; }

    public string? Role { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? IdentityNumber { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<DoctorTeam> DoctorTeams { get; set; } = new List<DoctorTeam>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<SampleCollectionSchedule> SampleCollectionSchedules { get; set; } = new List<SampleCollectionSchedule>();

    public virtual ICollection<SampleShipment> SampleShipments { get; set; } = new List<SampleShipment>();

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserWorkSchedule> UserWorkSchedules { get; set; } = new List<UserWorkSchedule>();
}
