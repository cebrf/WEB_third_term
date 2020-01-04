using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sorrow.Data;
using Sorrow.Models;

namespace Sorrow.Controllers
{
    [Route("api/[controller]")]  // /api/Items
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemContext _context;

        public ItemsController(ItemContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem()
        {
            return await _context.Item.ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<Item> PostItem(Item item)
        {
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetItem", new { id = item.Id }, item);
            return item;
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}


/*
 * 
 * @{
    ViewData["Title"] = "Home Page";
}
    <script src="~/js/jquery-1.7.1.min.js"></script>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <table id="tblPatients" class="table" cellpadding="0" cellspacing="0">
        <thead>
            <tr>
                <th style="width:100px">Patient Id</th>
                <th style="width:150px">Name</th>
                <th style="width:150px">Diagnosis</th>
                <th style="width:150px"></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="Id">
                    <span></span>
                </td>
                <td class="Name">
                    <span></span>
                    <input type="text" value="" style="display:none" />
                </td>
                <td class="Diagnosis">
                    <span></span>
                    <input type="text" value="" style="display:none" />
                </td>
                <td>
                    <a class="Edit" href="javascript:;">Edit</a>
                    <a class="Update" href="javascript:;" style="display:none">Update</a>
                    <a class="Cancel" href="javascript:;" style="display:none">Cancel</a>
                    <a class="Delete" href="javascript:;">Delete</a>
                </td>
            </tr>
        </tbody>
    </table>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 150px" class="form-group">
                <label for="name">Name</label>
                <input type="text" class="form-control" id="name" />
            </td>
            <td style="width: 150px" class="form-group">
                <label for="diagnosis">Diagnosis</label>
                <input type="text" class="form-control" id="diagnosis" />
            </td>
            <td style="width: 200px">
                <br />
                <input type="button" id="create" value="Create" />
            </td>
        </tr>
    </table>
    <div>
        <input type="text" class="form-control" id="postResult" />
        <button class="btn btn-default" style="width:140px" id="getPeople">Get People</button>
    </div>

    <script type="text/javascript">
        $(function () {
            var row = $("#tblPatients tbody tr:last-child");
            if ($("#tblItems tr").length > 2) {
                //$("#tblItems tr:eq(1)").remove();
            } else {
                var row = $("#tblItems tbody tr:last-child");
                row.find(".Edit").hide();
                row.find(".Delete").hide();
                row.find("span").html('&nbsp;');
            }
        });

        function AppendRow(row, Id, Name, Diagnosis) {
            $("#postResult").val(Id + "  " + Name + "  " + Diagnosis);

            $(".Id", row).find("span").html(Id);

            $(".Name", row).find("span").html(Name);
            $(".Name", row).find("input").val(Name);

            $(".Diagnosis", row).find("span").html(Diagnosis);
            $(".Diagnosis", row).find("input").val(Diagnosis);

            row.find(".Edit").show();
            row.find(".Delete").show();

            $("#tblPatients").append(row);
        };

        $('#getPeople').click(function (e) {
            $("#tblPatients").find("tr:gt(1)").remove();

            $("#postResult").val("");
            $.ajax({
                contentType: 'application/json',
                type: "GET",
                url: "api/Patients",
                success: function (data, textStatus, jqXHR) {
                    data.forEach(function (person) {
                        $("#postResult").val($("#postResult").val() + person.name + "|");

                        var row = $("#tblPatients tbody tr:last-child");
                        row = row.clone();
                        AppendRow(row, person.id, person.name, person.diagnosis);
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("#postResult").val(jqXHR.statusText);
                }
            });
        });

        $('#create').click(function (e) {
            $.ajax({
                contentType: 'application/json',
                type: "POST",
                url: "api/Patients",
                data: JSON.stringify({
                    name: document.getElementById('name').value,
                    diagnosis: document.getElementById('diagnosis').value
                }),
                success: function (person) {
                    $("#postResult").val(person.name + "|");

                    var row = $("#tblItems tbody tr:last-child");
                    row = row.clone();
                    AppendRow(row, person.id, person.name, person.diagnosis);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("#postResult").val(document.getElementById('name').value +
                        "  " + document.getElementById('diagnosis').value + "  " + jqXHR.statusText);
                }
            });
        });
    </script>
</body>
</html>
 * 
 */
