﻿namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profile3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "ProfilePic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "ProfilePic", c => c.String());
        }
    }
}
