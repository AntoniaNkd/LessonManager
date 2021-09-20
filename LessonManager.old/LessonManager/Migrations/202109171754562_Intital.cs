namespace LessonManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Role = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeachClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        ExamWeight = c.Single(nullable: false),
                        LabWeight = c.Single(nullable: false),
                        ExamMandatory = c.Boolean(nullable: false),
                        LabMandatory = c.Boolean(nullable: false),
                        Lesson_Id = c.Int(),
                        Teacher_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.Lesson_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.Teacher_Id)
                .Index(t => t.Lesson_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.String(),
                        Lesson_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.Lesson_Id)
                .Index(t => t.Lesson_Id);
            
            CreateTable(
                "dbo.Dilosis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExamMark = c.Single(nullable: false),
                        LabMark = c.Single(nullable: false),
                        TeachClass_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TeachClasses", t => t.TeachClass_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.User_Id)
                .Index(t => t.TeachClass_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dilosis", "User_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Dilosis", "TeachClass_Id", "dbo.TeachClasses");
            DropForeignKey("dbo.TeachClasses", "Teacher_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.TeachClasses", "Lesson_Id", "dbo.Lessons");
            DropForeignKey("dbo.Lessons", "Lesson_Id", "dbo.Lessons");
            DropIndex("dbo.Dilosis", new[] { "User_Id" });
            DropIndex("dbo.Dilosis", new[] { "TeachClass_Id" });
            DropIndex("dbo.Lessons", new[] { "Lesson_Id" });
            DropIndex("dbo.TeachClasses", new[] { "Teacher_Id" });
            DropIndex("dbo.TeachClasses", new[] { "Lesson_Id" });
            DropTable("dbo.Dilosis");
            DropTable("dbo.Lessons");
            DropTable("dbo.TeachClasses");
            DropTable("dbo.ApplicationUsers");
        }
    }
}
