namespace PAM.DataN.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Search : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Search", "Index");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Search", "Index", c => c.String());
        }
    }
}
