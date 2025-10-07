using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserForm.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campuses",
                columns: table => new
                {
                    CampusId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CampusCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CampusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Campuses__FD598DD666C9DC05", x => x.CampusId);
                });

            migrationBuilder.CreateTable(
                name: "FormStatuses",
                columns: table => new
                {
                    FormStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FormStat__4B86ED318C51C456", x => x.FormStatusId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    NotificationTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__299002C104FAF234", x => x.NotificationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                columns: table => new
                {
                    PaymentStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentS__34F8AC3F7FD3491C", x => x.PaymentStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__8AFACE1AE7D395FA", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ServiceCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ServicePrice = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Services__C51BB00A972460D4", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    StudentID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StudentName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CampusId = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4C79C63B77", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Campus",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "CampusId");
                    table.ForeignKey(
                        name: "FK_Users_Role",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "FormRegisterService",
                columns: table => new
                {
                    FormId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SupportCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CampusId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true),
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    FormStatusId = table.Column<int>(type: "integer", nullable: false),
                    OriginalFormId = table.Column<Guid>(type: "uuid", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AdminNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true),
                    RejectedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FormRegi__FB05B7DD824FD58D", x => x.FormId);
                    table.ForeignKey(
                        name: "FK_Form_ApprovedBy",
                        column: x => x.ApprovedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Form_Campus",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "CampusId");
                    table.ForeignKey(
                        name: "FK_Form_Original",
                        column: x => x.OriginalFormId,
                        principalTable: "FormRegisterService",
                        principalColumn: "FormId");
                    table.ForeignKey(
                        name: "FK_Form_RejectedBy",
                        column: x => x.RejectedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Form_Service",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId");
                    table.ForeignKey(
                        name: "FK_Form_Status",
                        column: x => x.FormStatusId,
                        principalTable: "FormStatuses",
                        principalColumn: "FormStatusId");
                    table.ForeignKey(
                        name: "FK_Form_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FormStatusHistory",
                columns: table => new
                {
                    HistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FormId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatusId = table.Column<int>(type: "integer", nullable: true),
                    ToStatusId = table.Column<int>(type: "integer", nullable: false),
                    ChangedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ChangeNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FormStat__4D7B4ABD10EDF276", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_FSH_ByUser",
                        column: x => x.ChangedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_FSH_Form",
                        column: x => x.FormId,
                        principalTable: "FormRegisterService",
                        principalColumn: "FormId");
                    table.ForeignKey(
                        name: "FK_FSH_From",
                        column: x => x.FromStatusId,
                        principalTable: "FormStatuses",
                        principalColumn: "FormStatusId");
                    table.ForeignKey(
                        name: "FK_FSH_To",
                        column: x => x.ToStatusId,
                        principalTable: "FormStatuses",
                        principalColumn: "FormStatusId");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    FormId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentStatusId = table.Column<int>(type: "integer", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(13,2)", nullable: true, computedColumnSql: "(\"Subtotal\"-\"DiscountAmount\")", stored: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "VND"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    PaidAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true),
                    PaymentRef = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Invoices__D796AAB57DD8DE6F", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Inv_Form",
                        column: x => x.FormId,
                        principalTable: "FormRegisterService",
                        principalColumn: "FormId");
                    table.ForeignKey(
                        name: "FK_Inv_Status",
                        column: x => x.PaymentStatusId,
                        principalTable: "PaymentStatuses",
                        principalColumn: "PaymentStatusId");
                    table.ForeignKey(
                        name: "FK_Inv_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    NotificationTypeId = table.Column<int>(type: "integer", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FormId = table.Column<Guid>(type: "uuid", nullable: true),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DeliveryChannel = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "IN_APP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__20CF2E12329C189A", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notif_Form",
                        column: x => x.FormId,
                        principalTable: "FormRegisterService",
                        principalColumn: "FormId");
                    table.ForeignKey(
                        name: "FK_Notif_Type",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "NotificationTypeId");
                    table.ForeignKey(
                        name: "FK_Notif_User",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SupportSessions",
                columns: table => new
                {
                    SupportSessionId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    FormId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdminUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EndedAt = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: true),
                    OutcomeCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    OutcomeNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SupportS__C3AF68EE8B19272D", x => x.SupportSessionId);
                    table.ForeignKey(
                        name: "FK_SS_Admin",
                        column: x => x.AdminUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_SS_Form",
                        column: x => x.FormId,
                        principalTable: "FormRegisterService",
                        principalColumn: "FormId");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    InvoiceItemId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    UnitPrice = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(23,2)", nullable: true, computedColumnSql: "(\"Qty\"*\"UnitPrice\")", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__InvoiceI__478FE09C814A0AD0", x => x.InvoiceItemId);
                    table.ForeignKey(
                        name: "FK_InvItem_Inv",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_InvItem_Svc",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Campuses__4D8A9715BD74B188",
                table: "Campuses",
                column: "CampusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Form_Campus",
                table: "FormRegisterService",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_OriginalForm",
                table: "FormRegisterService",
                column: "OriginalFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_Service",
                table: "FormRegisterService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_Status",
                table: "FormRegisterService",
                column: "FormStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_User",
                table: "FormRegisterService",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRegisterService_ApprovedByUserId",
                table: "FormRegisterService",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRegisterService_RejectedByUserId",
                table: "FormRegisterService",
                column: "RejectedByUserId");

            migrationBuilder.CreateIndex(
                name: "UQ__FormStat__6A7B44FC87380979",
                table: "FormStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormStatusHistory_ChangedByUserId",
                table: "FormStatusHistory",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatusHistory_FromStatusId",
                table: "FormStatusHistory",
                column: "FromStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatusHistory_ToStatusId",
                table: "FormStatusHistory",
                column: "ToStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FSH_Form",
                table: "FormStatusHistory",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ServiceId",
                table: "InvoiceItems",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Form",
                table: "Invoices",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PaymentStatusId",
                table: "Invoices",
                column: "PaymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UserId",
                table: "Invoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notif_User",
                table: "Notifications",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_FormId",
                table: "Notifications",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ__Notifica__3E1CDC7CACEC0AAD",
                table: "NotificationTypes",
                column: "TypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__PaymentS__6A7B44FC6CB969B4",
                table: "PaymentStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Roles__8A2B616004AAB738",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Services__A01D74C9B896996F",
                table: "Services",
                column: "ServiceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SS_Form",
                table: "SupportSessions",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportSessions_AdminUserId",
                table: "SupportSessions",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Campus",
                table: "Users",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Users_StudentID",
                table: "Users",
                column: "StudentID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormStatusHistory");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SupportSessions");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "FormRegisterService");

            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "FormStatuses");

            migrationBuilder.DropTable(
                name: "Campuses");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
