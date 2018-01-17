namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Points", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Points", "Task_Id1", "dbo.Tasks");
            DropIndex("dbo.Points", "IX_Task_Id1");
            DropColumn("dbo.Points", "Task_Id1");
          
            AddForeignKey("dbo.Points", "TaskId", "dbo.Tasks", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Points", "TaskId", "dbo.Tasks");
         
            AddColumn("dbo.Points", "Task_Id1", c => c.Int());
            CreateIndex("dbo.Points", "Task_Id1");
            AddForeignKey("dbo.Points", "Task_Id1", "dbo.Tasks", "Id");
            AddForeignKey("dbo.Points", "TaskId", "dbo.Tasks", "Id");
        }
    }
}
