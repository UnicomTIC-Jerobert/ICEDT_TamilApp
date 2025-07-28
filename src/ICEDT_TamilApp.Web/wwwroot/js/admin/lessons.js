// ~/js/admin/lessons.js

document.addEventListener('DOMContentLoaded', function () {
    // --- Configuration & Element References ---
    const tableBody = document.getElementById('tableBody');
    const addNewBtn = document.getElementById('addNewBtn');
    const newEntryTfoot = document.getElementById('newEntryTfoot');
    const dynamicHeader = document.getElementById('dynamicHeader');
    const adminContainer = document.querySelector('.admin-container');
    const entityName = 'Lesson';

    // --- Get Level ID from URL ---
    // This script requires the page URL to have a query parameter like: /Admin/Lessons?levelId=1
    const urlParams = new URLSearchParams(window.location.search);
    const levelId = urlParams.get('levelId');

    // --- API Endpoints ---
    // These URLs are constructed to match the specific route attributes in your LessonController.cs
    const getLessonsApiUrl = `/lessons/${levelId}`; // Matches [HttpGet("/lessons/{levelId:int}")]
    const createLessonApiUrl = `/api/lesson/level/${levelId}/lessons`; // Matches [HttpPost("level/{levelId:int}/lessons")]
    const updateLessonApiUrl = (lessonId) => `/api/lesson/lessons/${lessonId}`; // Matches [HttpPut("lessons/{id}")]
    const deleteLessonApiUrl = (lessonId) => `/api/lesson/lessons/${lessonId}`; // Matches [HttpDelete("lessons/{id}")]


    // --- INITIALIZATION ---
    // A levelId is mandatory. If it's not in the URL, stop execution and show an error.
    if (!levelId || isNaN(parseInt(levelId))) {
        adminContainer.innerHTML = `
            <h1 class="admin-header mb-0 text-danger">Error: No Level Selected</h1>
            <p class="admin-description">A valid Level ID must be provided in the URL to manage lessons.</p>
            <p>Example: <code>/Admin/Lessons?levelId=1</code></p>
            <a href="/Admin/Levels" class="btn btn-primary mt-3"><i class="fas fa-arrow-left"></i> Go Back to Levels</a>`;
        return; // Stop all further script execution
    }
    fetchData();


    // --- RENDER FUNCTION ---
    // Takes lesson data and builds the HTML table rows
    function renderTable(lessons) {
        tableBody.innerHTML = '';
        if (!lessons || lessons.length === 0) {
            tableBody.innerHTML = `<tr><td colspan="5" class="text-center">No lessons found for this level. You can add one!</td></tr>`;
            return;
        }

        // Sort lessons by sequenceOrder just in case they are not pre-sorted
        lessons.sort((a, b) => a.sequenceOrder - b.sequenceOrder);

        lessons.forEach(item => {
            const row = document.createElement('tr');
            row.setAttribute('data-id', item.lessonId);
            row.innerHTML = `
                <td>${item.lessonId}</td>
                <td data-field="lessonName">${item.lessonName}</td>
                <td data-field="description">${item.description || ''}</td>
                <td data-field="sequenceOrder">${item.sequenceOrder}</td>
                <td class="actions-column">
                    <button class="btn btn-sm btn-primary btn-edit">Edit</button>
                    <button class="btn btn-sm btn-danger btn-delete">Delete</button>
                    <button class="btn btn-sm btn-success btn-save" style="display:none;">Save</button>
                    <button class="btn btn-sm btn-secondary btn-cancel" style="display:none;">Cancel</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    }

    // --- API CALLS ---
    async function fetchData() {
        try {
            tableBody.innerHTML = '<tr><td colspan="5" class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></td></tr>';
            const response = await fetch(getLessonsApiUrl);
            if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

            // The API returns an object { levelName: '...', lessons: [...] }
            const result = await response.json();
            dynamicHeader.textContent = `Manage Lessons for: ${result.levelName}`;
            renderTable(result.lessons);

        } catch (error) {
            console.error(`Error fetching ${entityName}s:`, error);
            dynamicHeader.textContent = 'Error Loading Lessons';
            tableBody.innerHTML = `<tr><td colspan="5" class="text-center text-danger">Could not load lessons. Check the browser console for details.</td></tr>`;
        }
    }

    async function createItem(payload) {
        try {
            const response = await fetch(createLessonApiUrl, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({ message: 'Failed to create lesson. The server returned an error.' }));
                throw new Error(errorData.message);
            }
            return true;
        } catch (error) {
            console.error(`Error creating ${entityName}:`, error);
            alert(`Error: ${error.message}`);
            return false;
        }
    }

    async function updateItem(id, payload) {
        try {
            const response = await fetch(updateLessonApiUrl(id), {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!response.ok) throw new Error('Failed to save changes. The server returned an error.');
            return true;
        } catch (error) {
            console.error(`Error updating ${entityName}:`, error);
            alert(`Error: ${error.message}`);
            return false;
        }
    }

    async function deleteItem(id) {
        try {
            const response = await fetch(deleteLessonApiUrl(id), {
                method: 'DELETE'
            });
            if (!response.ok) throw new Error('Failed to delete the lesson. The server returned an error.');
            return true;
        } catch (error) {
            console.error(`Error deleting ${entityName}:`, error);
            alert(`Error: ${error.message}`);
            return false;
        }
    }

    // --- EVENT HANDLING ---
    // Use a single event listener on the container for efficiency (event delegation)
    adminContainer.addEventListener('click', async function (e) {
        const target = e.target;

        if (target.id === 'addNewBtn') {
            showAddNewRow();
            return;
        }

        const row = target.closest('tr');
        if (!row) return; // Exit if the click was not inside a table row

        // Handle buttons inside the main table body
        if (tableBody.contains(target)) {
            if (target.classList.contains('btn-edit')) enterEditMode(row);
            else if (target.classList.contains('btn-save')) await exitEditMode(row, true);
            else if (target.classList.contains('btn-cancel')) exitEditMode(row, false);
            else if (target.classList.contains('btn-delete')) await handleDelete(row);
        }
        // Handle buttons inside the "add new" footer
        else if (newEntryTfoot.contains(target)) {
            if (target.classList.contains('btn-save-new')) await handleSaveNew();
            else if (target.classList.contains('btn-cancel-new')) hideAddNewRow();
        }
    });

    // --- ADD/EDIT/DELETE LOGIC ---

    function showAddNewRow() {
        newEntryTfoot.style.display = 'table-footer-group';
        addNewBtn.style.display = 'none';
        document.getElementById('newName').focus();
    }

    function hideAddNewRow() {
        newEntryTfoot.querySelectorAll('input').forEach(input => input.value = '');
        newEntryTfoot.style.display = 'none';
        addNewBtn.style.display = 'block';
    }

    async function handleSaveNew() {
        const payload = {
            lessonName: document.getElementById('newName').value.trim(),
            description: document.getElementById('newDescription').value.trim(),
            sequenceOrder: parseInt(document.getElementById('newOrder').value, 10)
        };

        if (!payload.lessonName || isNaN(payload.sequenceOrder)) {
            alert('Lesson Name and a valid Order number are required.');
            return;
        }

        const success = await createItem(payload);
        if (success) {
            hideAddNewRow();
            await fetchData(); // Refresh the table to show the new item
        }
    }

    async function handleDelete(row) {
        const id = row.dataset.id;
        const name = row.querySelector('[data-field="lessonName"]').textContent;
        if (!confirm(`Are you sure you want to delete the lesson "${name}"?`)) return;

        const success = await deleteItem(id);
        if (success) {
            row.remove();
        }
    }

    const fields = ['lessonName', 'description', 'sequenceOrder'];

    function enterEditMode(row) {
        // Cancel any other row that might be in edit mode
        const currentlyEditing = document.querySelector('tr[data-original-data]');
        if (currentlyEditing && currentlyEditing !== row) {
            exitEditMode(currentlyEditing, false);
        }

        const originalData = {};
        fields.forEach(field => {
            const cell = row.querySelector(`[data-field="${field}"]`);
            originalData[field] = cell.textContent;
            const inputType = field === 'sequenceOrder' ? 'number' : 'text';
            cell.innerHTML = `<input type="${inputType}" class="form-control" value="${cell.textContent}">`;
        });
        row.dataset.originalData = JSON.stringify(originalData); // Store original state
        row.querySelector('[data-field="lessonName"] input').focus();
        toggleButtons(row, true);
    }

    async function exitEditMode(row, shouldSave) {
        const id = row.dataset.id;
        const originalData = JSON.parse(row.dataset.originalData);

        if (shouldSave) {
            const payload = {
                lessonName: row.querySelector('[data-field="lessonName"] input').value.trim(),
                description: row.querySelector('[data-field="description"] input').value.trim(),
                sequenceOrder: parseInt(row.querySelector('[data-field="sequenceOrder"] input').value, 10)
            };
            if (!payload.lessonName || isNaN(payload.sequenceOrder)) {
                alert('Lesson Name and a valid Order number are required.');
                return; // Don't exit edit mode if validation fails
            }
            const success = await updateItem(id, payload);
            if (!success) return; // Stay in edit mode if save fails
        }

        // Restore view (either with new data from payload or original data)
        fields.forEach(field => {
            const cell = row.querySelector(`[data-field="${field}"]`);
            const value = shouldSave
                ? row.querySelector(`[data-field="${field}"] input`).value
                : originalData[field];
            cell.textContent = value;
        });

        row.removeAttribute('data-original-data');
        toggleButtons(row, false);
    }

    function toggleButtons(row, isEditing) {
        row.querySelector('.btn-edit').style.display = isEditing ? 'none' : 'inline-block';
        row.querySelector('.btn-delete').style.display = isEditing ? 'none' : 'inline-block';
        row.querySelector('.btn-save').style.display = isEditing ? 'inline-block' : 'none';
        row.querySelector('.btn-cancel').style.display = isEditing ? 'inline-block' : 'none';
    }
});