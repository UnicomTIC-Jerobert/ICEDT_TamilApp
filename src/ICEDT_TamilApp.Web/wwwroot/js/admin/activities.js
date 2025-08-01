document.addEventListener('DOMContentLoaded', function () {
    // --- CONFIGURATION ---
    const config = {
        entityName: 'Activity',
        tableBodyId: 'tableBody',
        addNewBtnId: 'addNewBtn',
        newEntryTfootId: 'newEntryTfoot',
        fields: [
            { name: 'title', type: 'text', required: true },
            { name: 'sequenceOrder', type: 'number', required: true },
            { name: 'contentJson', type: 'textarea', required: true },
            { name: 'activityTypeId', type: 'number', required: true },
            { name: 'mainActivityId', type: 'number', required: true }
        ],
    };

    const urlParams = new URLSearchParams(window.location.search);
    const lessonId = urlParams.get('lessonId');

    // --- CORRECTED API ENDPOINTS ---
    // These now match the routes in your ActivitiesController.cs
    const getApiUrl = `/api/activities/by-lesson?lessonId=${lessonId}`;
    const createApiUrl = '/api/activities';
    const updateApiUrl = (id) => `/api/activities/${id}`;
    const deleteApiUrl = (id) => `/api/activities/${id}`;

    const tableBody = document.getElementById(config.tableBodyId);
    const addNewBtn = document.getElementById(config.addNewBtnId);
    const newEntryTfoot = document.getElementById(config.newEntryTfootId);
    const adminContainer = document.querySelector('.admin-container');

    // --- INITIALIZATION ---
    if (!lessonId) {
        adminContainer.innerHTML = `<h1 class="text-danger">Error: No Lesson ID</h1><p>Please return to the lessons page and select a lesson to manage.</p><a href="/Admin/Lessons" class="btn btn-primary">Back to Lessons</a>`;
        return;
    }
    fetchData();
    document.getElementById('backButtonContainer').innerHTML = `<a href="/Admin/Lessons" class="btn btn-secondary"><i class="fas fa-arrow-left"></i> Back to All Lessons</a>`;


    // --- RENDER FUNCTION ---
    function renderTable(items) {
        tableBody.innerHTML = '';
        const colspan = config.fields.length + 2; // Fields + ID + Actions
        if (!items || items.length === 0) {
            tableBody.innerHTML = `<tr><td colspan="${colspan}" class="text-center">No activities found for this lesson.</td></tr>`;
            return;
        }
        items.sort((a, b) => a.sequenceOrder - b.sequenceOrder);
        items.forEach(item => {
            const row = document.createElement('tr');
            row.setAttribute('data-id', item.activityId);
            let cells = `<td>${item.activityId}</td>`;
            config.fields.forEach(field => {
                const value = item[field.name] === null || item[field.name] === undefined ? '' : item[field.name];
                cells += `<td data-field="${field.name}">${value}</td>`;
            });
            row.innerHTML = `${cells}
                <td class="actions-column">
                    <button class="btn btn-sm btn-primary btn-edit">Edit</button>
                    <button class="btn btn-sm btn-danger btn-delete">Delete</button>
                    <button class="btn btn-sm btn-success btn-save" style="display:none;">Save</button>
                    <button class="btn btn-sm btn-secondary btn-cancel" style="display:none;">Cancel</button>
                </td>`;
            tableBody.appendChild(row);
        });
    }

    // --- EVENT HANDLING ---
    adminContainer.addEventListener('click', async function (e) {
        const target = e.target;
        const row = target.closest('tr');
        if (target.id === config.addNewBtnId) {
            showAddNewRow();
        } else if (row) {
            if (tableBody.contains(target)) {
                if (target.classList.contains('btn-edit')) enterEditMode(row);
                else if (target.classList.contains('btn-save')) await exitEditMode(row, true);
                else if (target.classList.contains('btn-cancel')) exitEditMode(row, false);
                else if (target.classList.contains('btn-delete')) await handleDelete(row);
            } else if (newEntryTfoot.contains(target)) {
                if (target.classList.contains('btn-save-new')) await handleSaveNew();
                else if (target.classList.contains('btn-cancel-new')) hideAddNewRow();
            }
        }
    });

    // --- FULL CRUD LOGIC ---
    function isValidJson(str) {
        if (str.trim() === '') return false;
        try { JSON.parse(str); } catch (e) { return false; }
        return true;
    }

    function showAddNewRow() {
        let maxOrder = 0;
        tableBody.querySelectorAll('tr[data-id]').forEach(row => {
            const currentOrder = parseInt(row.querySelector('[data-field="sequenceOrder"]').textContent, 10);
            if (!isNaN(currentOrder) && currentOrder > maxOrder) maxOrder = currentOrder;
        });
        document.getElementById('newOrder').value = maxOrder + 1;
        newEntryTfoot.style.display = 'table-footer-group';
        addNewBtn.style.display = 'none';
        document.getElementById('newTitle').focus();
    }

    function hideAddNewRow() {
        newEntryTfoot.querySelectorAll('input, textarea').forEach(el => el.value = '');
        newEntryTfoot.style.display = 'none';
        addNewBtn.style.display = 'block';
    }

    async function handleSaveNew() {
        const payload = {
            title: document.getElementById('newTitle').value.trim(),
            sequenceOrder: parseInt(document.getElementById('newOrder').value, 10),
            contentJson: document.getElementById('newContentJson').value.trim(),
            activityTypeId: parseInt(document.getElementById('newActivityTypeId').value, 10),
            mainActivityId: parseInt(document.getElementById('newMainActivityId').value, 10),
            lessonId: parseInt(lessonId, 10),
        };
        if (!payload.title || isNaN(payload.sequenceOrder) || isNaN(payload.activityTypeId) || isNaN(payload.mainActivityId)) {
            alert('Title, Order, Type ID, and Main ID are required and must be numbers.');
            return;
        }
        if (!isValidJson(payload.contentJson)) {
            alert('Content must be valid JSON.');
            return;
        }
        const success = await createItem(payload);
        if (success) {
            hideAddNewRow();
            await fetchData();
        }
    }

    async function handleDelete(row) {
        const id = row.dataset.id;
        if (!confirm(`Are you sure you want to delete Activity ID ${id}?`)) return;
        const success = await deleteItem(id);
        if (success) row.remove();
    }

    function enterEditMode(row) {
        const currentlyEditing = document.querySelector('tr[data-original-data]');
        if (currentlyEditing && currentlyEditing !== row) {
            exitEditMode(currentlyEditing, false);
        }
        const originalData = {};
        config.fields.forEach(field => {
            const cell = row.querySelector(`[data-field="${field.name}"]`);
            originalData[field.name] = cell.textContent;
            let inputElement;
            if (field.type === 'textarea') {
                inputElement = `<textarea class="form-control" rows="4">${cell.textContent}</textarea>`;
            } else {
                inputElement = `<input type="${field.type}" class="form-control" value="${cell.textContent}">`;
            }
            cell.innerHTML = inputElement;
        });
        row.dataset.originalData = JSON.stringify(originalData);
        row.querySelector('[data-field="title"] .form-control').focus();
        toggleButtons(row, true);
    }

    async function exitEditMode(row, shouldSave) {
        const id = row.dataset.id;
        if (shouldSave) {
            const payload = { lessonId: parseInt(lessonId, 10) };
            let hasError = false;
            config.fields.forEach(field => {
                const element = row.querySelector(`[data-field="${field.name}"] .form-control`);
                const value = element.value.trim();
                if (field.type === 'number') {
                    payload[field.name] = parseInt(value, 10);
                    if (isNaN(payload[field.name]) && field.required) hasError = true;
                } else {
                    payload[field.name] = value;
                    if (!value && field.required) hasError = true;
                }
            });
            if (hasError) {
                alert('All fields are required and numbers must be valid.');
                return;
            }
            if (!isValidJson(payload.contentJson)) {
                alert('Content must be valid JSON.');
                return;
            }
            const success = await updateItem(id, payload);
            if (!success) return;
        }
        const originalData = JSON.parse(row.dataset.originalData);
        config.fields.forEach(field => {
            const cell = row.querySelector(`[data-field="${field.name}"]`);
            const input = cell.querySelector('.form-control');
            cell.textContent = shouldSave ? input.value : originalData[field.name];
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

    // --- FULLY IMPLEMENTED API CALLS ---
    async function fetchData() {
        try {
            const colspan = config.fields.length + 2;
            tableBody.innerHTML = `<tr><td colspan="${colspan}" class="text-center"><div class="spinner-border" role="status"></div></td></tr>`;
            const response = await fetch(getApiUrl);
            if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
            const items = await response.json();
            renderTable(items);
        } catch (error) {
            const colspan = config.fields.length + 2;
            console.error(`Error fetching ${config.entityName}s:`, error);
            tableBody.innerHTML = `<tr><td colspan="${colspan}" class="text-center text-danger">Could not load activities.</td></tr>`;
        }
    }
    async function createItem(payload) {
        try {
            const response = await fetch(createApiUrl, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({ message: 'Failed to create item. Check console for server response.' }));
                throw new Error(errorData.message || errorData.title);
            }
            return true;
        } catch (error) {
            alert(`Error: ${error.message}`);
            return false;
        }
    }
    async function updateItem(id, payload) {
        try {
            const response = await fetch(updateApiUrl(id), { method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({ message: 'Failed to save changes.' }));
                throw new Error(errorData.message || errorData.title);
            }
            return true;
        } catch (error) {
            alert(`Error: ${error.message}`);
            return false;
        }
    }
    async function deleteItem(id) {
        try {
            const response = await fetch(deleteApiUrl(id), { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete item.');
            return true;
        } catch (error) {
            alert(`Error: ${error.message}`);
            return false;
        }
    }
});