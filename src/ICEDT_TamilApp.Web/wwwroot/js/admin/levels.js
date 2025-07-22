document.addEventListener('DOMContentLoaded', function () {
    const tableBody = document.getElementById('levelsTableBody');

    // 1. Function to load levels from the API
    async function loadLevels() {
        try {
            const response = await fetch('/api/levels');
            if (!response.ok) throw new Error('Failed to fetch levels');
            const levels = await response.json();

            tableBody.innerHTML = ''; // Clear existing rows
            levels.forEach(level => {
                const row = document.createElement('tr');
                row.setAttribute('data-level-id', level.levelId);
                row.innerHTML = `
                    <td>${level.levelId}</td>
                    <td data-field="levelName">${level.levelName}</td>
                    <td data-field="sequenceOrder">${level.sequenceOrder}</td>
                    <td>
                        <button class="btn btn-sm btn-primary btn-edit">Edit</button>
                        <button class="btn btn-sm btn-success btn-save" style="display:none;">Save</button>
                        <button class="btn btn-sm btn-secondary btn-cancel" style="display:none;">Cancel</button>
                    </td>
                `;
                tableBody.appendChild(row);
            });
        } catch (error) {
            console.error('Error loading levels:', error);
            alert('Could not load levels.');
        }
    }

    // 2. Event listener for the entire table body (Event Delegation)
    tableBody.addEventListener('click', function (e) {
        const target = e.target;
        const row = target.closest('tr');
        if (!row) return;

        if (target.classList.contains('btn-edit')) {
            handleEdit(row);
        } else if (target.classList.contains('btn-save')) {
            handleSave(row);
        } else if (target.classList.contains('btn-cancel')) {
            handleCancel(row);
        }
    });

    // 3. Functions to handle button clicks
    function handleEdit(row) {
        // Store original values in case of cancel
        row.setAttribute('data-original-name', row.querySelector('[data-field="levelName"]').textContent);
        row.setAttribute('data-original-order', row.querySelector('[data-field="sequenceOrder"]').textContent);
        
        // Convert cells to input fields
        row.querySelectorAll('[data-field]').forEach(cell => {
            const value = cell.textContent;
            cell.innerHTML = `<input type="text" class="form-control" value="${value}">`;
        });
        
        // Toggle buttons
        toggleButtons(row, true);
    }

    async function handleSave(row) {
        const levelId = row.dataset.levelId;
        const levelNameInput = row.querySelector('[data-field="levelName"] input');
        const sequenceOrderInput = row.querySelector('[data-field="sequenceOrder"] input');

        const payload = {
            levelName: levelNameInput.value,
            sequenceOrder: parseInt(sequenceOrderInput.value, 10)
        };
        
        try {
            const response = await fetch(`/api/levels/${levelId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if (!response.ok) throw new Error('Failed to save changes');
            
            // Update UI with new values
            levelNameInput.parentElement.innerHTML = payload.levelName;
            sequenceOrderInput.parentElement.innerHTML = payload.sequenceOrder;
            
            toggleButtons(row, false);
            alert('Level updated successfully!');
        } catch (error) {
            console.error('Error saving level:', error);
            alert('Failed to save. Please check console.');
        }
    }

    function handleCancel(row) {
        // Revert to original values
        row.querySelector('[data-field="levelName"]').innerHTML = row.dataset.originalName;
        row.querySelector('[data-field="sequenceOrder"]').innerHTML = row.dataset.originalOrder;
        
        toggleButtons(row, false);
    }

    function toggleButtons(row, isEditing) {
        row.querySelector('.btn-edit').style.display = isEditing ? 'none' : 'inline-block';
        row.querySelector('.btn-save').style.display = isEditing ? 'inline-block' : 'none';
        row.querySelector('.btn-cancel').style.display = isEditing ? 'inline-block' : 'none';
    }

    // Initial load
    loadLevels();
});