namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Points", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Points", "Task_Id", "dbo.Tasks");
            DropIndex("dbo.Points", new[] { "Task_Id" });
            DropColumn("dbo.Points", "TaskId");
            RenameColumn(table: "dbo.Points", name: "Task_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Points", name: "Task_Id1", newName: "Task_Id");
            RenameColumn(table: "dbo.Points", name: "__mig_tmp__0", newName: "TaskId");
            RenameIndex(table: "dbo.Points", name: "IX_Task_Id1", newName: "IX_Task_Id");
            AddForeignKey("dbo.Points", "TaskId", "dbo.Tasks", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Points", "TaskId", "dbo.Tasks");
            RenameIndex(table: "dbo.Points", name: "IX_Task_Id", newName: "IX_Task_Id1");
            RenameColumn(table: "dbo.Points", name: "TaskId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Points", name: "Task_Id", newName: "Task_Id1");
            RenameColumn(table: "dbo.Points", name: "__mig_tmp__0", newName: "Task_Id");
            AddColumn("dbo.Points", "TaskId", c => c.Int());
            CreateIndex("dbo.Points", "Task_Id");
            AddForeignKey("dbo.Points", "Task_Id", "dbo.Tasks", "Id");
            AddForeignKey("dbo.Points", "TaskId", "dbo.Tasks", "Id");
        }
    }
}
