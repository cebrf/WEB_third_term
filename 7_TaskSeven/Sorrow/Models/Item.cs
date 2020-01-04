using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sorrow.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }
    }
}


/*
 * 
<div class="container">
    <div class="row">
        <div class="col-sm-6">
            <h2>Get all people</h2>
            <div>
                <button class="btn btn-default" id="getPeople">Get People</button>
            </div>
            <div>
                <input type="text" class="form-control" id="getPeopleResult" />
            </div>
        </div>
    </div>
</div>

<script>
    $('#getPeople').click(function (e) {
        $("#getPeopleResult").val("");
        $.ajax({
            contentType: 'application/json',
            type: "GET",
            url: "api/Items",
            success: function (data, textStatus, jqXHR) {
                data.forEach(function (person) {
                    $("#getPeopleResult").val($("#getPeopleResult").val() + person.name + "|");
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#getPeopleResult").val(jqXHR.statusText);
            }
        });
    });
</script>



<div class="container">
    <div class="row">
        <div class="col-sm-6">
            <h2>Get a Person</h2>
            <div class="form-group">
                <label for="id">Id</label>
                <input type="text" class="form-control" id="id" />
            </div>
            <div>
                <button class="btn btn-default" id="getPerson">Get</button>
            </div>
            <div>
                <button class="btn btn-default" id="deletePerson">Delete</button>
            </div>
            <div>
                <input type="text" class="form-control" id="getPersonResult" />
            </div>
        </div>

    </div>
</div>

<script>
    $('#getPerson').click(function (e) {
        $("#getPersonResult").val("");
        $.ajax({
            contentType: 'application/json',
            type: "GET",
            url: "api/Items/" + $("#id").val(),
            success: function (data, textStatus, jqXHR) {
                $("#getPersonResult").val(data.name);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#getPersonResult").val(jqXHR.statusText);
            }
        });
    });
</script>

<script>
    $('#deletePerson').click(function (e) {
        $("#getPersonResult").val("");
        $.ajax({
            contentType: 'application/json',
            type: "DELETE",
            url: "api/Items/" + $("#id").val(),
            success: function (data, textStatus, jqXHR) {
                $("#getPersonResult").val("Person deleted");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#getPersonResult").val(jqXHR.statusText);
            }
        });
    });
</script>


<div class="container">
    <div class="row">
        <div class="col-sm-6">
            <h2>Create a new Person</h2>
            <div class="form-group">
                <label for="name">Name</label>
                <input type="text" class="form-control" id="name" />
            </div>
            <div class="form-group">
                <label for="emailAddress">Email</label>
                <input type="email" class="form-control" id="emailAddress" />
            </div>
            <div>
                <button class="btn btn-default" id="create">Create</button>
            </div>
            <div>
                <input type="text" class="form-control" id="postResult" />
            </div>
        </div>
    </div>
</div>
<script>
    $('#create').click(function (e) {
        $.ajax({
            contentType: 'application/json',
            type: "POST",
            url: "api/Items",
            data: JSON.stringify({
                name: document.getElementById('name').value
            }),
            success: function (data, textStatus, jqXHR) {
                $("#postResult").val("Person created ok. Id=" + jqXHR.responseText);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#postResult").val(jqXHR.statusText);
            }
        });
    });
</script>

    */