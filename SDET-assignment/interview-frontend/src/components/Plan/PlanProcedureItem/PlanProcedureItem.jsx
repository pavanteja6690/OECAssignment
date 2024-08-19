import React, { useEffect, useState } from "react";
import ReactSelect from "react-select";
import { assignUsersToPlanProcedure } from "../../../api/api";

const PlanProcedureItem = ({ procedure, users, planId, planProceduresSelectedUsers }) => {
    const [selectedUsers, setSelectedUsers] = useState(null);

    useEffect(() => {
        if (planProceduresSelectedUsers) {
            setSelectedUsers(planProceduresSelectedUsers);
        }
    }, [planProceduresSelectedUsers]);

    const handleAssignUserToProcedure = async (e) => {
        setSelectedUsers(e);
        const userIds = e.length > 0 ? e.map(y => y.value) : [];
        await assignUsersToPlanProcedure(planId, procedure.procedureId, userIds);
    };

    return (
        <div className="py-2">
            <div>
                {procedure.procedureTitle}
            </div>

            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={(e) => handleAssignUserToProcedure(e)}
            />
        </div>
    );
};

export default PlanProcedureItem;
