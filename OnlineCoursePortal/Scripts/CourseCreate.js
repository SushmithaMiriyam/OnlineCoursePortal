$(document).ready(function () {
    $sectiondropdown = $('#totalSections');

    $sectiondropdown.on("change", function (e) {
        var tot = $(this).val();
        $('#Sections').html(' ')
        for (var i = 1; i <= tot; i++) {
            $('#Sections').append('<div class="form-group">'+
                                    'Section "+i+":<br />'+
                                    '<select id="totalLectures1">'+
                    <option value="0">--Select--</option>
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="5">5</option>
                </select>)
        }
        


        
        <div class="form-group">
            <label> Total Number of Sections:</label>
            <select name="totalSections" id="totalSections">
                <option value="0">--Select--</option>
                <option value="1">1</option>
                <option value="2">2</option>
                <option value="3">3</option>
                <option value="4">4</option>
                <option value="5">5</option>
            </select>
        </div>
        <div id="Sections">


            <div class="form-group">
                Section 1:<br />
                <select name="totalLectures1" id="totalLectures1">
                    <option value="0">--Select--</option>
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="5">5</option>
                </select>
                <input type="file" name="lecture1" accept="video/*">
                <input type="file" name="lecture2" accept="video/*">
                <input type="file" name="lecture3" accept="video/*">
                <input type="file" name="lecture4" accept="video/*">
                <input type="file" name="lecture5" accept="video/*">
            </div>
        </div>
    });
});