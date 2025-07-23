document.addEventListener('DOMContentLoaded', function () {
    const tableBody = document.getElementById('levelsTableBody');
    const addNewLevelBtn = document.getElementById('addNewLevelBtn');
    const newLevelTfoot = document.getElementById('newLevelTfoot');

    // --- RENDER FUNCTION ---
    // Renders the table rows from the data fetched from the API
    // *** MODIFIED: Add a Delete button to each row ***
    function renderTable(levels) {
        tableBody.innerHTML = '';
        if (!levels || levels.length === 0) {
            tableBody.innerHTML = '<tr><td colspan="4" class="text-center">No levels found.</td></tr>';
            return;
        }

        levels.forEach(level => {
            const row = document.createElement('tr');
            row.setAttribute('data-level-id', level.levelId);
            // Added btn-danger for delete
            row.innerHTML = `
                <td>${level.levelId}</td>
                <td data-field="levelName">${level.levelName}</td>
                <td data-field="sequenceOrder">${level.sequenceOrder}</td>
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
    // Fetches all levels from the API
    async function fetchLevels() {
        try {
            const response = await fetch('/api/levels');
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            const levels = await response.json();
            renderTable(levels);
        } catch (error) {
            console.error('Error fetching levels:', error);
            tableBody.innerHTML = '<tr><td colspan="4" class="text-center text-danger">Could not load levels.</td></tr>';
        }
    }

    // Updates a level via a PUT request
    async function updateLevel(levelId, payload) {
        try {
            const response = await fetch(`/api/levels/${levelId}`, {
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
            console.error('Error updating level:', error);
            alert(`Error: ${error.message}`);
            return false;
        }
    }

    async function createLevel(payload) {
        try {
            const response = await fetch('/api/levels', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.title || 'Failed to create level');
            }
            return true;
        } catch (error) {
            console.error('Error creating level:', error);
            alert(`Error: ${error.message}`);
            return false;
        }
    }

    async function deleteLevel(levelId) {
        try {
            const response = await fetch(`/api/levels/${levelId}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                // Handle cases where the server returns an error (e.g., 404 Not Found)
                const errorText = response.status === 404 ? "Level not found." : "Failed to delete.";
                throw new Error(errorText);
            }
            return true;
        } catch (error) {
            console.error('Error deleting level:', error);
            alert(`Error: ${error.message}`);
            return false;
        }
    }

    // --- EVENT HANDLING ---
    // *** MODIFIED: Handle new buttons ***
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
                await handleDelete(row); // New handler
            }
        }

        // Handle global buttons
        if (target.id === 'addNewLevelBtn') {
            showAddNewRow();
        }
        // Handle buttons within the tfoot
        else if (newLevelTfoot.contains(target) && row) {
            if (target.classList.contains('btn-save-new')) {
                await handleSaveNew();
            } else if (target.classList.contains('btn-cancel-new')) {
                hideAddNewRow();
            }
        }
    });

    // --- DELETE LOGIC ---
    // *** NEW: Function to handle the delete process ***
    async function handleDelete(row) {
        const levelId = row.dataset.levelId;
        const levelName = row.querySelector('[data-field="levelName"]').textContent;

        // Use a confirmation dialog for safety
        if (!confirm(`Are you sure you want to delete the level "${levelName}"? This action cannot be undone.`)) {
            return;
        }

        const success = await deleteLevel(levelId);

        if (success) {
            row.remove(); // Remove the row from the UI immediately for a responsive feel
            alert('Level deleted successfully!');
            // Optional: You could call fetchLevels() again, but removing the row directly is faster UX.
        }
    }
    // --- ADD NEW LOGIC ---
    function showAddNewRow() {
        newLevelTfoot.style.display = 'table-footer-group';
        addNewLevelBtn.style.display = 'none'; // Hide the "Add" button while the form is open
        document.getElementById('newLevelName').focus();
    }

    function hideAddNewRow() {
        document.getElementById('newLevelName').value = '';
        document.getElementById('newLevelOrder').value = '';
        newLevelTfoot.style.display = 'none';
        addNewLevelBtn.style.display = 'block';
    }

    async function handleSaveNew() {
        const newNameInput = document.getElementById('newLevelName');
        const newOrderInput = document.getElementById('newLevelOrder');

        const payload = {
            levelName: newNameInput.value.trim(),
            sequenceOrder: parseInt(newOrderInput.value, 10)
        };

        if (!payload.levelName || isNaN(payload.sequenceOrder)) {
            alert('Level Name cannot be empty and Sequence Order must be a number.');
            return;
        }

        const success = await createLevel(payload);
        if (success) {
            hideAddNewRow();
            await fetchLevels(); // Refresh the entire table to show the new entry
            alert('Level created successfully!');
        }
    }



    // --- EVENT HANDLING ---
    // Uses event delegation for edit, save, and cancel buttons
    tableBody.addEventListener('click', async function (e) {
        const target = e.target;
        const row = target.closest('tr');
        if (!row) return;

        if (target.classList.contains('btn-edit')) {
            enterEditMode(row);
        } else if (target.classList.contains('btn-save')) {
            await exitEditMode(row, true); // true = save changes
        } else if (target.classList.contains('btn-cancel')) {
            exitEditMode(row, false); // false = discard changes
        }
    });

    // --- EDIT MODE LOGIC ---
    // Converts a row's cells into editable input fields
    function enterEditMode(row) {
        // Store original values on the row element for cancellation
        const nameCell = row.querySelector('[data-field="levelName"]');
        const orderCell = row.querySelector('[data-field="sequenceOrder"]');
        row.setAttribute('data-original-name', nameCell.textContent);
        row.setAttribute('data-original-order', orderCell.textContent);

        nameCell.innerHTML = `<input type="text" class="form-control" value="${nameCell.textContent}">`;
        orderCell.innerHTML = `<input type="number" class="form-control" value="${orderCell.textContent}">`;

        toggleButtons(row, true);
    }

    // Handles saving or canceling the edit
    async function exitEditMode(row, shouldSave) {
        const levelId = row.dataset.levelId;
        const nameCell = row.querySelector('[data-field="levelName"]');
        const orderCell = row.querySelector('[data-field="sequenceOrder"]');

        if (shouldSave) {
            const nameInput = nameCell.querySelector('input');
            const orderInput = orderCell.querySelector('input');

            const payload = {
                levelName: nameInput.value.trim(),
                sequenceOrder: parseInt(orderInput.value, 10)
            };

            // Basic validation
            if (!payload.levelName || isNaN(payload.sequenceOrder)) {
                alert('Level Name cannot be empty and Sequence Order must be a number.');
                return;
            }

            const success = await updateLevel(levelId, payload);
            if (success) {
                nameCell.textContent = payload.levelName;
                orderCell.textContent = payload.sequenceOrder;
                toggleButtons(row, false);
            }
        } else {
            // Revert to original values on cancel
            nameCell.textContent = row.dataset.originalName;
            orderCell.textContent = row.dataset.originalOrder;
            toggleButtons(row, false);
        }
    }

    // Toggles the visibility of Edit/Save/Cancel buttons
    function toggleButtons(row, isEditing) {
        row.querySelector('.btn-edit').style.display = isEditing ? 'none' : 'inline-block';
        row.querySelector('.btn-delete').style.display = isEditing ? 'none' : 'inline-block'; // Hide delete during edit
        row.querySelector('.btn-save').style.display = isEditing ? 'inline-block' : 'none';
        row.querySelector('.btn-cancel').style.display = isEditing ? 'inline-block' : 'none';
    }

    // --- INITIALIZATION ---
    // Fetch and render the levels when the page loads
    fetchLevels();
});