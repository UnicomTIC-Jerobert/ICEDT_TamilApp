document.addEventListener('DOMContentLoaded', function () {
    // --- Configuration ---
    const config = {
        apiEndpoint: '/api/mainactivities',
        tableBodyId: 'tableBody',
        addNewBtnId: 'addNewBtn',
        newEntryTfootId: 'newEntryTfoot',
        entityName: 'Main Activity',
        fields: [
            { name: 'name', type: 'text' }
        ]
    };

    const tableBody = document.getElementById(config.tableBodyId);
    const addNewBtn = document.getElementById(config.addNewBtnId);
    const newEntryTfoot = document.getElementById(config.newEntryTfootId);

    // --- RENDER FUNCTION ---
    function renderTable(items) {
        tableBody.innerHTML = '';
        if (!items || items.length === 0) {
            tableBody.innerHTML = `<tr><td colspan="3" class="text-center">No ${config.entityName}s found.</td></tr>`;
            return;
        }

        items.forEach(item => {
            const row = document.createElement('tr');
            row.setAttribute('data-id', item.id);
            let cells = `<td>${item.id}</td>`;
            config.fields.forEach(field => {
                cells += `<td data-field="${field.name}">${item[field.name]}</td>`;
            });

            row.innerHTML = `
                ${cells}
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
    async function fetchData() { /* Generic fetch using config.apiEndpoint */ }
    async function createItem(payload) { /* Generic POST using config.apiEndpoint */ }
    async function updateItem(id, payload) { /* Generic PUT using config.apiEndpoint/{id} */ }
    async function deleteItem(id) { /* Generic DELETE using config.apiEndpoint/{id} */ }

    // Implement the generic API calls here based on the `levels.js` pattern...

    document.addEventListener('DOMContentLoaded', function () {
        const tableBody = document.getElementById('tableBody');
        const addNewBtn = document.getElementById('addNewBtn');
        const newEntryTfoot = document.getElementById('newEntryTfoot');
        const apiEndpoint = '/api/mainactivities';
        const entityName = 'Main Activity';

        // --- RENDER FUNCTION ---
        // Renders the table rows from the data fetched from the API
        function renderTable(items) {
            tableBody.innerHTML = ''; // Clear existing rows
            if (!items || items.length === 0) {
                tableBody.innerHTML = `<tr><td colspan="3" class="text-center">No ${entityName}s found.</td></tr>`;
                return;
            }

            items.forEach(item => {
                const row = document.createElement('tr');
                row.setAttribute('data-id', item.id);
                row.innerHTML = `
                <td>${item.id}</td>
                <td data-field="name">${item.name}</td>
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
        // Fetches all items from the API
        async function fetchData() {
            try {
                tableBody.innerHTML = '<tr><td colspan="3" class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></td></tr>';
                const response = await fetch(apiEndpoint);
                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
                const items = await response.json();
                renderTable(items);
            } catch (error) {
                console.error(`Error fetching ${entityName}s:`, error);
                tableBody.innerHTML = `<tr><td colspan="3" class="text-center text-danger">Could not load ${entityName}s.</td></tr>`;
            }
        }

        // Creates a new item via a POST request
        async function createItem(payload) {
            try {
                const response = await fetch(apiEndpoint, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload)
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.title || `Failed to create ${entityName}`);
                }
                return true;
            } catch (error) {
                console.error(`Error creating ${entityName}:`, error);
                alert(`Error: ${error.message}`);
                return false;
            }
        }

        // Updates an item via a PUT request
        async function updateItem(id, payload) {
            try {
                const response = await fetch(`${apiEndpoint}/${id}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload)
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.title || 'Failed to save changes');
                }
                return true;
            } catch (error) {
                console.error(`Error updating ${entityName}:`, error);
                alert(`Error: ${error.message}`);
                return false;
            }
        }

        // Deletes an item via a DELETE request
        async function deleteItem(id) {
            try {
                const response = await fetch(`${apiEndpoint}/${id}`, {
                    method: 'DELETE'
                });
                if (!response.ok) {
                    const errorText = response.status === 404 ? `${entityName} not found.` : "Failed to delete.";
                    throw new Error(errorText);
                }
                return true;
            } catch (error) {
                console.error(`Error deleting ${entityName}:`, error);
                alert(`Error: ${error.message}`);
                return false;
            }
        }

        // --- EVENT HANDLING ---
        // Uses event delegation for all button clicks
        document.body.addEventListener('click', async function (e) {
            const target = e.target;
            const row = target.closest('tr');

            // Handle buttons within the main table body
            if (tableBody.contains(target) && row) {
                if (target.classList.contains('btn-edit')) {
                    enterEditMode(row);
                } else if (target.classList.contains('btn-save')) {
                    await exitEditMode(row, true);
                } else if (target.classList.contains('btn-cancel')) {
                    exitEditMode(row, false);
                } else if (target.classList.contains('btn-delete')) {
                    await handleDelete(row);
                }
            }

            // Handle global "Add New" button
            if (target.id === 'addNewBtn') {
                showAddNewRow();
            }
            // Handle buttons within the tfoot for new entry
            else if (newEntryTfoot.contains(target) && row) {
                if (target.classList.contains('btn-save-new')) {
                    await handleSaveNew();
                } else if (target.classList.contains('btn-cancel-new')) {
                    hideAddNewRow();
                }
            }
        });

        // --- DELETE LOGIC ---
        async function handleDelete(row) {
            const id = row.dataset.id;
            const name = row.querySelector('[data-field="name"]').textContent;

            if (!confirm(`Are you sure you want to delete "${name}"? This action cannot be undone.`)) {
                return;
            }
            const success = await deleteItem(id);
            if (success) {
                row.remove();
                alert(`${entityName} deleted successfully!`);
            }
        }

        // --- ADD NEW LOGIC ---
        function showAddNewRow() {
            newEntryTfoot.style.display = 'table-footer-group';
            addNewBtn.style.display = 'none';
            document.getElementById('newName').focus();
        }

        function hideAddNewRow() {
            document.getElementById('newName').value = '';
            newEntryTfoot.style.display = 'none';
            addNewBtn.style.display = 'block';
        }

        async function handleSaveNew() {
            const newNameInput = document.getElementById('newName');
            const payload = {
                name: newNameInput.value.trim(),
            };

            if (!payload.name) {
                alert('Name cannot be empty.');
                return;
            }

            const success = await createItem(payload);
            if (success) {
                hideAddNewRow();
                await fetchData(); // Refresh the table
                alert(`${entityName} created successfully!`);
            }
        }

        // --- EDIT MODE LOGIC ---
        function enterEditMode(row) {
            const nameCell = row.querySelector('[data-field="name"]');
            row.setAttribute('data-original-name', nameCell.textContent);

            nameCell.innerHTML = `<input type="text" class="form-control" value="${nameCell.textContent}">`;

            toggleButtons(row, true);
        }

        async function exitEditMode(row, shouldSave) {
            const id = row.dataset.id;
            const nameCell = row.querySelector('[data-field="name"]');

            if (shouldSave) {
                const nameInput = nameCell.querySelector('input');
                const payload = {
                    name: nameInput.value.trim(),
                };

                if (!payload.name) {
                    alert('Name cannot be empty.');
                    return;
                }

                const success = await updateItem(id, payload);
                if (success) {
                    nameCell.textContent = payload.name;
                    toggleButtons(row, false);
                }
            } else {
                nameCell.textContent = row.dataset.originalName;
                toggleButtons(row, false);
            }
        }

        function toggleButtons(row, isEditing) {
            row.querySelector('.btn-edit').style.display = isEditing ? 'none' : 'inline-block';
            row.querySelector('.btn-delete').style.display = isEditing ? 'none' : 'inline-block';
            row.querySelector('.btn-save').style.display = isEditing ? 'inline-block' : 'none';
            row.querySelector('.btn-cancel').style.display = isEditing ? 'inline-block' : 'none';
        }

        // --- INITIALIZATION ---
        fetchData();
    });
});