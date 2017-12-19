namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Points", "TaskId", "dbo.Tasks");
            AddColumn("dbo.Points", "X", c => c.Double(nullable: false));
            AddColumn("dbo.Points", "Y", c => c.Double(nullable: false));
            AddColumn("dbo.Points", "Task_Id", c => c.Int());
            AddColumn("dbo.Points", "Task_Id1", c => c.Int());
            AddColumn("dbo.Users", "Token", c => c.String());
            AddColumn("dbo.Users", "TokenCreationTime", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Points", "Task_Id");
            CreateIndex("dbo.Points", "Task_Id1");
            AddForeignKey("dbo.Points", "Task_Id1", "dbo.Tasks", "Id");
            AddForeignKey("dbo.Points", "Task_Id", "dbo.Tasks", "Id");
            DropColumn("dbo.Points", "Rank");
            DropColumn("dbo.Points", "Address");
            DropColumn("dbo.Points", "Shape");
            DropColumn("dbo.Tasks", "Route");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "Route", c => c.Geometry());
            AddColumn("dbo.Points", "Shape", c => c.Geometry());
            AddColumn("dbo.Points", "Address", c => c.String());
            AddColumn("dbo.Points", "Rank", c => c.Int(nullable: false));
            DropForeignKey("dbo.Points", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.Points", "Task_Id1", "dbo.Tasks");
            DropIndex("dbo.Points", new[] { "Task_Id1" });
            DropIndex("dbo.Points", new[] { "Task_Id" });
            DropColumn("dbo.Users", "TokenCreationTime");
            DropColumn("dbo.Users", "Token");
            DropColumn("dbo.Points", "Task_Id1");
            DropColumn("dbo.Points", "Task_Id");
            DropColumn("dbo.Points", "Y");
            DropColumn("dbo.Points", "X");
            AddForeignKey("dbo.Points", "TaskId", "dbo.Tasks", "Id");
        }
    }
}
